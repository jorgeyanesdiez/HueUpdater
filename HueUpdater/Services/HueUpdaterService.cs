using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Factories;
using HueUpdater.Models;
using HueUpdater.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HueUpdater.Services
{

    /// <summary>
    /// Orchestrates application services to update a Hue endpoint.
    /// </summary>
    public class HueUpdaterService
        : BackgroundService
    {

        /// <summary>
        /// Allows integration with the container's lifetime.
        /// </summary>
        private IHostApplicationLifetime AppLifetime { get; }


        /// <summary>
        /// A logger for this service.
        /// </summary>
        private ILogger Logger { get; }


        /// <summary>
        /// The service required to determine whether a build is taking place.
        /// </summary>
        private IResolver<CIActivityStatus[], CIActivityStatus> ActivityStatusResolver { get; }


        /// <summary>
        /// The service required to determine whether builds are broken or stable.
        /// </summary>
        private IResolver<CIBuildStatus[], CIBuildStatus> BuildStatusResolver { get; }


        /// <summary>
        /// The service required to determine whether the lamps should be flashing.
        /// </summary>
        private IResolver<CIStatusChangeQuery, HueAlert> HueAlertResolver { get; }


        /// <summary>
        /// The service required to determine the color of the lamps.
        /// </summary>
        private IResolver<CIStatus, HueColor> HueColorResolver { get; }


        /// <summary>
        /// The service required to modify the state of the lamps.
        /// </summary>
        private IHueInvoker HueInvoker { get; }


        /// <summary>
        /// The service required to to access the file with the last build status.
        /// </summary>
        private ISerializer Serializer { get; }


        /// <summary>
        /// The service required to determine whether a given schedule is applicable.
        /// </summary>
        private IResolver<ScheduleQuery, bool> ScheduleApplicabilityResolver { get; }


        /// <summary>
        /// The service required to get a schedule.
        /// </summary>
        private IResolver<DateTime, (string Name, TimeRangeSettings Times)> ScheduleResolver { get; }


        /// <summary>
        /// The services required to get activity information from different endpoints.
        /// </summary>
        private IEnumerable<IActivityStatusProvider<Task<CIActivityStatus>>> ActivityStatusProviders { get; }


        /// <summary>
        /// The services required to get build status information from different endpoints.
        /// </summary>
        private IEnumerable<IBuildStatusProvider<Task<CIBuildStatus>>> BuildStatusProviders { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="appLifetime">The value for the <see cref="AppLifetime"/> property.</param>
        /// <param name="logger">The value for the <see cref="Logger"/> property.</param>
        /// <param name="activityStatusResolver">The value for the <see cref="ActivityStatusResolver"/> property.</param>
        /// <param name="buildStatusResolver">The value for the <see cref="BuildStatusResolver"/> property.</param>
        /// <param name="hueAlertResolver">The value for the <see cref="HueAlertResolver"/> property.</param>
        /// <param name="hueColorResolver">The value for the <see cref="HueColorResolver"/> property.</param>
        /// <param name="hueInvoker">The value for the <see cref="HueInvoker"/> property.</param>
        /// <param name="serializer">The value for the <see cref="Serializer"/> property.</param>
        /// <param name="scheduleApplicabilityResolver">The value for the <see cref="ScheduleApplicabilityResolver"/> property.</param>
        /// <param name="scheduleResolver">The value for the <see cref="ScheduleResolver"/> property.</param>
        /// <param name="activityStatusProviders">The value for the <see cref="ActivityStatusProviders"/> property.</param>
        /// <param name="buildStatusProviders">The value for the <see cref="BuildStatusProviders"/> property.</param>
        public HueUpdaterService(
            IHostApplicationLifetime appLifetime,
            ILogger<HueUpdaterService> logger,
            IResolver<CIActivityStatus[], CIActivityStatus> activityStatusResolver,
            IResolver<CIBuildStatus[], CIBuildStatus> buildStatusResolver,
            IResolver<CIStatusChangeQuery, HueAlert> hueAlertResolver,
            IResolver<CIStatus, HueColor> hueColorResolver,
            IHueInvoker hueInvoker,
            ISerializer serializer,
            IResolver<ScheduleQuery, bool> scheduleApplicabilityResolver,
            IResolver<DateTime, (string Name, TimeRangeSettings Times)> scheduleResolver,
            IEnumerable<IActivityStatusProvider<Task<CIActivityStatus>>> activityStatusProviders,
            IEnumerable<IBuildStatusProvider<Task<CIBuildStatus>>> buildStatusProviders
        )
        {
            AppLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            ActivityStatusResolver = activityStatusResolver ?? throw new ArgumentNullException(nameof(activityStatusResolver));
            BuildStatusResolver = buildStatusResolver ?? throw new ArgumentNullException(nameof(buildStatusResolver));
            HueAlertResolver = hueAlertResolver ?? throw new ArgumentNullException(nameof(hueAlertResolver));
            HueColorResolver = hueColorResolver ?? throw new ArgumentNullException(nameof(hueColorResolver));
            HueInvoker = hueInvoker ?? throw new ArgumentNullException(nameof(hueInvoker));
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            ScheduleApplicabilityResolver = scheduleApplicabilityResolver ?? throw new ArgumentNullException(nameof(scheduleApplicabilityResolver));
            ScheduleResolver = scheduleResolver ?? throw new ArgumentNullException(nameof(scheduleResolver));
            ActivityStatusProviders = activityStatusProviders ?? throw new ArgumentNullException(nameof(activityStatusProviders));
            if (!activityStatusProviders.Any()) { throw new ArgumentOutOfRangeException(nameof(activityStatusProviders)); }
            BuildStatusProviders = buildStatusProviders ?? throw new ArgumentNullException(nameof(buildStatusProviders));
            if (!buildStatusProviders.Any()) { throw new ArgumentOutOfRangeException(nameof(buildStatusProviders)); }
        }


        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken _)
        {
            try { await UpdateHueEndpointAsync(); }
            catch (Exception exc)
            {
                Logger.LogError("An error was encountered: {ExceptionType}", exc.GetType());
                throw;
            }
            finally { AppLifetime.StopApplication(); }
        }


        /// <summary>
        /// Orchestrates all required services to update the Hue lamp.
        /// </summary>
        /// <returns>The task required for async compatibility.</returns>
        public async Task UpdateHueEndpointAsync()
        {
            var dateTime = DateTime.Now;
            var (scheduleName, scheduleTime) = ScheduleResolver.Resolve(dateTime.Date);
            var isScheduleApplicable = ScheduleApplicabilityResolver.Resolve(new ScheduleQuery { ScheduleName = scheduleName, Time = dateTime.TimeOfDay });
            Logger.LogInformation("Schedule: {ScheduleName} | {ScheduleTimeStart} - {ScheduleTimeFinish}", scheduleName, scheduleTime.Start, scheduleTime.Finish);
            Logger.LogInformation("Time: {Timestamp} | Power-On schedule applicable? {ScheduleApplicable}", $"{dateTime:HH:mm}", isScheduleApplicable);

            if (isScheduleApplicable)
            {
                // Get the current status
                var activityStatusCalls = ActivityStatusProviders.Select(a => a.GetActivityStatus());
                var buildStatusCalls = BuildStatusProviders.Select(b => b.GetBuildStatus());
                var currentStatus = new CIStatus
                {
                    ActivityStatus = ActivityStatusResolver.Resolve(await Task.WhenAll(activityStatusCalls)),
                    BuildStatus = BuildStatusResolver.Resolve(await Task.WhenAll(buildStatusCalls))
                };

                // Get the previous status
                var previousStatus = Serializer.Deserialize<CIStatus>();

                // Save the current status
                Serializer.Serialize(currentStatus);

                // Get the settings for the current status
                var hueColor = HueColorResolver.Resolve(currentStatus);
                var hueAlert = HueAlertResolver.Resolve(new CIStatusChangeQuery { Current = currentStatus, Previous = previousStatus });
                Logger.LogInformation("Status: {ActivityStatus} - {BuildStatus}", currentStatus.ActivityStatus, currentStatus.BuildStatus);

                // Update the endpoint
                await HueInvoker.PutAsync(hueColor);
                if (hueAlert != null) { await HueInvoker.PutAsync(hueAlert); }
            }
            else
            {
                await HueInvoker.PutAsync(HuePowerFactory.CreateOff());
            }
        }

    }

}
