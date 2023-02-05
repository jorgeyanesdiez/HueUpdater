using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
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
        /// The URLs to get CI information from.
        /// </summary>
        private List<string> CIStatusUrls { get; }


        /// <summary>
        /// The service required to reduce multiple CI status values to a single one.
        /// </summary>
        private IResolver<CIStatus[], CIStatus> CIStatusReducer { get; }


        /// <summary>
        /// The service required to determine whether a light should flash.
        /// </summary>
        private IResolver<LightStatusChangeQuery, HueAlert> HueAlertResolver { get; }


        /// <summary>
        /// The items to be processed by this service.
        /// Each item represents an endpoint to be updated.
        /// </summary>
        private IEnumerable<HueUpdaterItem> HueUpdaterItems { get; }


        /// <summary>
        /// The service required to determine the color of the light.
        /// </summary>
        private IResolver<CIStatus, LightColor> LightColorResolver { get; }


        /// <summary>
        /// The service used to persist and recover the status of the light.
        /// </summary>
        private ISerializer<LightStatus> Serializer { get; }


        /// <summary>
        /// The service required to determine whether a given schedule is applicable.
        /// </summary>
        private IResolver<ScheduleQuery, bool> ScheduleApplicabilityResolver { get; }


        /// <summary>
        /// The service required to get a schedule.
        /// </summary>
        private IResolver<DateTime, (string Name, ScheduleSettings Schedule)> ScheduleResolver { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="appLifetime">The value for the <see cref="AppLifetime"/> property.</param>
        /// <param name="logger">The value for the <see cref="Logger"/> property.</param>
        /// <param name="ciStatusUrls">The value for the <see cref="CIStatusUrls"/> property.</param>
        /// <param name="ciStatusReducer">The value for the <see cref="CIStatusReducer"/> property.</param>
        /// <param name="hueAlertResolver">The value for the <see cref="HueAlertResolver"/> property.</param>
        /// <param name="hueUpdaterItems">The value for the <see cref="HueUpdaterItems"/> property.</param>
        /// <param name="lightColorResolver">The value for the <see cref="LightColorResolver"/> property.</param>
        /// <param name="serializer">The value for the <see cref="Serializer"/> property.</param>
        /// <param name="scheduleApplicabilityResolver">The value for the <see cref="ScheduleApplicabilityResolver"/> property.</param>
        /// <param name="scheduleResolver">The value for the <see cref="ScheduleResolver"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public HueUpdaterService(
            IHostApplicationLifetime appLifetime,
            ILogger<HueUpdaterService> logger,
            List<string> ciStatusUrls,
            IResolver<CIStatus[], CIStatus> ciStatusReducer,
            IResolver<LightStatusChangeQuery, HueAlert> hueAlertResolver,
            IEnumerable<HueUpdaterItem> hueUpdaterItems,
            IResolver<CIStatus, LightColor> lightColorResolver,
            ISerializer<LightStatus> serializer,
            IResolver<ScheduleQuery, bool> scheduleApplicabilityResolver,
            IResolver<DateTime, (string Name, ScheduleSettings Schedule)> scheduleResolver
        )
        {
            AppLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            CIStatusUrls = ciStatusUrls ?? throw new ArgumentNullException(nameof(ciStatusUrls));
            if (!ciStatusUrls.Any()) { throw new ArgumentOutOfRangeException(nameof(ciStatusUrls)); }
            CIStatusReducer = ciStatusReducer ?? throw new ArgumentNullException(nameof(ciStatusReducer));
            HueAlertResolver = hueAlertResolver ?? throw new ArgumentNullException(nameof(hueAlertResolver));
            HueUpdaterItems = hueUpdaterItems ?? throw new ArgumentNullException(nameof(hueUpdaterItems));
            if (!hueUpdaterItems.Any()) { throw new ArgumentOutOfRangeException(nameof(hueUpdaterItems)); }
            LightColorResolver = lightColorResolver ?? throw new ArgumentNullException(nameof(lightColorResolver));
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            ScheduleApplicabilityResolver = scheduleApplicabilityResolver ?? throw new ArgumentNullException(nameof(scheduleApplicabilityResolver));
            ScheduleResolver = scheduleResolver ?? throw new ArgumentNullException(nameof(scheduleResolver));
        }


        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken _)
        {
            var processTask = ProcessItemsAsync();
            try { await processTask; }
            catch (Exception exc)
            {
                var e = processTask.Exception?.Flatten() ?? exc;
                Logger.LogError("Error: {ExceptionMessage}", e.Message);
                throw e;
            }
            finally { AppLifetime.StopApplication(); }
        }


        /// <summary>
        /// Orchestrates services to process each defined item.
        /// </summary>
        /// <returns>The task context.</returns>
        public async Task ProcessItemsAsync()
        {
            var dateTime = DateTime.Now;
            var (ScheduleName, Schedule) = ScheduleResolver.Resolve(dateTime.Date);
            var withinWorkingHours = ScheduleApplicabilityResolver.Resolve(new ScheduleQuery { Time = dateTime.TimeOfDay, Schedule = Schedule });
            Logger.LogInformation("Schedule: {ScheduleName} | {ScheduleTimeStart} - {ScheduleTimeFinish}", ScheduleName, Schedule.Hours.Start, Schedule.Hours.Finish);
            Logger.LogInformation("Time: {TimeofDay} | Within working hours? {WithinWorkingHours}", $"{dateTime:HH:mm}", withinWorkingHours);

            var currentStatus = new LightStatus() { Power = LightPower.Off, Color = null };
            if (withinWorkingHours)
            {
                var ciStatusCollection = await Task.WhenAll(CIStatusUrls.Select(async srcUrl => await srcUrl.GetJsonAsync<CIStatus>()));
                var ciStatus = CIStatusReducer.Resolve(ciStatusCollection);
                Logger.LogInformation("CI Status: {ActivityStatus} - {BuildStatus}", ciStatus.ActivityStatus, ciStatus.BuildStatus);

                currentStatus.Power = LightPower.On;
                currentStatus.Color = LightColorResolver.Resolve(ciStatus);
            }

            var previousStatus = Serializer.Deserialize();
            Serializer.Serialize(currentStatus);
            Logger.LogInformation("Light Status: {Power} - {Color}", currentStatus.Power, currentStatus.Color?.ToString() ?? "No color");

            var hueAlert = HueAlertResolver.Resolve(new LightStatusChangeQuery { Previous = previousStatus, Current = currentStatus });
            Logger.LogInformation("Alerting?: {Alert}", currentStatus.Color.HasValue && hueAlert != null);

            await Task.WhenAll(HueUpdaterItems.Select(async item => await ProcessItemAsync(currentStatus, item.HueColorFactory, hueAlert, item.HueInvoker)));
        }


        /// <summary>
        /// Sends the status to a service item.
        /// </summary>
        /// <param name="status">The status of the light.</param>
        /// <param name="hueColorFactory">The service to create the DTO for the color.</param>
        /// <param name="hueAlert">Optional DTO for the alert.</param>
        /// <param name="hueInvoker">The service to contact the endpoints.</param>
        /// <returns></returns>
        public async Task ProcessItemAsync(
            LightStatus status,
            IResolver<LightColor, HueColor> hueColorFactory,
            HueAlert hueAlert,
            IHueInvoker hueInvoker
        )
        {
            if (status.Color.HasValue)
            {
                var hueColor = hueColorFactory.Resolve(status.Color.Value);
                await hueInvoker.PutAsync(hueColor);
                if (hueAlert != null) { await hueInvoker.PutAsync(hueAlert); }
            }
            else
            {
                await hueInvoker.PutAsync(HuePowerFactory.CreateOff());
            }
        }

    }

}
