using System;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Models;
using HueUpdater.Services;
using HueUpdater.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static Microsoft.Extensions.DependencyInjection.ActivatorUtilities;

namespace HueUpdater
{

    /// <summary>
    /// The program's entry point.
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Entry point from the command line.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }


        /// <summary>
        /// Creates and configures a host builder for later use.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The requested host builder.</returns>
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Environment.CurrentDirectory);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.AddJsonFile("appsettings.json");
                    configApp.AddUserSecrets<Program>(optional: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var appSettings = hostContext.Configuration.Get<AppSettings>();

                    // Application services DI
                    services.AddSingleton<IResolver<CIActivityStatus[], CIActivityStatus>, CIActivityStatusResolver>();
                    services.AddSingleton<IResolver<CIBuildStatus[], CIBuildStatus>, CIBuildStatusResolver>();
                    services.AddSingleton<IResolver<CIStatusChangeQuery, HueAlert>, HueAlertResolver>();
                    services.AddSingleton<IResolver<CIStatus, HueColor>, HueColorResolver>();
                    services.AddSingleton<IHueInvoker>(sp => CreateInstance<HueInvoker>(sp, appSettings.Hue.Endpoint));
                    services.AddSingleton<Abstractions.ISerializer>(sp => CreateInstance<JsonNetFileSerializer>(sp, appSettings.Persistence.LastStatusFilePath));
                    services.AddSingleton<IResolver<ScheduleQuery, bool>>(sp => CreateInstance<ScheduleApplicabilityResolver>(sp, appSettings.Operation.Schedules));
                    services.AddSingleton<IResolver<DateTime, string>>(sp => CreateInstance<ScheduleNameResolver>(sp, appSettings.Operation.Calendar));
                    services.AddSingleton<IResolver<DateTime, (string Name, TimeRangeSettings Times)>>(sp => CreateInstance<ScheduleResolver>(sp, appSettings.Operation.Schedules));

                    // Status services DI - May eventually use an assembly scanner to make these pluggable.
                    services.AddSingleton(sp => CreateInstance<JenkinsStatusAggregator>(sp, appSettings.Jenkins.BaseEndpoint, appSettings.Jenkins.JobNameRegexFilter));
                    services.AddSingleton<IActivityStatusProvider<Task<CIActivityStatus>>>(sp => sp.GetRequiredService<JenkinsStatusAggregator>());
                    services.AddSingleton<IBuildStatusProvider<Task<CIBuildStatus>>>(sp => sp.GetRequiredService<JenkinsStatusAggregator>());

                    // Hosted service DI
                    services.AddHostedService<HueUpdaterService>();

                    // Application services configuration
                    ConfigureAppServices(appSettings.Jenkins);
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime();
        }


        /// <summary>
        /// Configures the application services that require configuration.
        /// </summary>
        /// <param name="jenkinsSettings">The settings required to configure Jenkins requests.</param>
        private static void ConfigureAppServices(JenkinsSettings jenkinsSettings)
        {
            var jsonSettings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            FlurlHttp.Configure(s => s.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings));

            if (!string.IsNullOrWhiteSpace(jenkinsSettings.User) || !string.IsNullOrWhiteSpace(jenkinsSettings.Password))
            {
                FlurlHttp.ConfigureClient(jenkinsSettings.BaseEndpoint, cl =>
                    cl.WithBasicAuth(jenkinsSettings.User, jenkinsSettings.Password)
                );
            }
            jenkinsSettings.Password = null;
        }

    }

}
