using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using HueUpdater.Factories;
using HueUpdater.Models;
using HueUpdater.Services;
using HueUpdater.Settings;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HueUpdater
{

    /// <summary>
    /// Contains the entry point from the command line.
    /// </summary>
    class Program
    {

        /// <summary>
        /// The entry point from the command line.
        /// </summary>
        /// <returns>Task for async compatibility.</returns>
        async static Task Main()
        {
            await new Program().RunAsync();
        }


        /// <summary>
        /// Runs the application's logic.
        /// </summary>
        /// <returns>Task for async compatibility.</returns>
        public async Task RunAsync()
        {
            var config = GetAppSettings();
            var scheduleResolver = new ScheduleResolver(config.Operation.Calendar);
            var applicabilityResolver = new ScheduleApplicabilityResolver(config.Operation.Schedule);
            var jenkinsAggregator = new JenkinsStatusAggregator(config.Jenkins.BaseEndpoint, config.Jenkins.JobNameRegexFilter);
            var teamCityAggregator = new TeamCityStatusAggregator(config.TeamCity.BaseEndpoint);
            var activityStatusResolver = new CIActivityStatusResolver();
            var buildStatusResolver = new CIBuildStatusResolver();
            var fileSerializer = new JsonNetFileSerializer(config.LastStatusFilePath);
            var hueColorResolver = new HueColorResolver();
            var hueAlertResolver = new HueAlertResolver();
            var hueInvoker = new HueInvoker(config.Hue.Endpoint);
            ConfigureServices(config);

            var dateTime = DateTime.Now;
            var schedule = scheduleResolver.Resolve(dateTime.Date) ?? config.Operation.Schedule.Keys.First();
            var isScheduleApplicable = applicabilityResolver.Resolve(new ScheduleQuery { ScheduleName = schedule, Time = dateTime.TimeOfDay });
            Console.WriteLine($"Schedule: {schedule} | {config.Operation.Schedule[schedule].Start} - {config.Operation.Schedule[schedule].Finish}");
            Console.WriteLine($"Time: {dateTime.ToString("HH:mm")} | Power enabled? {isScheduleApplicable}");

            if (isScheduleApplicable)
            {
                var jenkinsActivityStatus = jenkinsAggregator.GetActivityStatus();
                var jenkinsBuildStatus = jenkinsAggregator.GetBuildStatus();
                var teamCityActivityStatus = teamCityAggregator.GetActivityStatus();
                var teamCityBuildStatus = teamCityAggregator.GetBuildStatus();
                var currentStatus = new CIStatus
                {
                    ActivityStatus = activityStatusResolver.Resolve(await jenkinsActivityStatus, await teamCityActivityStatus),
                    BuildStatus = buildStatusResolver.Resolve(await jenkinsBuildStatus, await teamCityBuildStatus)
                };
                var previousStatus = fileSerializer.Deserialize<CIStatus>();
                fileSerializer.Serialize(currentStatus);
                var hueColor = hueColorResolver.Resolve(currentStatus);
                var hueAlert = hueAlertResolver.Resolve(new CIStatusChangeQuery { Current = currentStatus, Previous = previousStatus });

                Console.WriteLine($"Jenkins status: { await jenkinsActivityStatus } - { await jenkinsBuildStatus }");
                Console.WriteLine($"TeamCity status: { await teamCityActivityStatus } - { await teamCityBuildStatus }");
                Console.WriteLine($"Global status: { currentStatus.ActivityStatus } - { currentStatus.BuildStatus }");

                await hueInvoker.PutAsync(hueColor);
                await hueInvoker.PutAsync(hueAlert);
            }
            else
            {
                await hueInvoker.PutAsync(HuePowerFactory.CreateOff());
            }
        }


        /// <summary>
        /// Configures the application's services.
        /// </summary>
        /// <param name="config">The settings required for configuration.</param>
        private void ConfigureServices(AppSettings config)
        {
            var jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            FlurlHttp.Configure(s => s.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings));

            FlurlHttp.ConfigureClient(config.Jenkins.BaseEndpoint, cl =>
                cl.WithBasicAuth(config.Jenkins.User, config.Jenkins.Password)
            );
            config.Jenkins.Password = null;

            FlurlHttp.ConfigureClient(config.TeamCity.BaseEndpoint, cl =>
                cl.WithHeader("Accept", "application/json")
                .WithBasicAuth(config.TeamCity.User, config.TeamCity.Password)
            );
            config.TeamCity.Password = null;
        }


        /// <summary>
        /// Obtains the application configuration from configured sources.
        /// </summary>
        /// <returns>The requested configuration.</returns>
        private AppSettings GetAppSettings()
        {
            var configRoot = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<Program>()
                .Build();

            var config = configRoot.Get<AppSettings>();
            return config;
        }

    }

}
