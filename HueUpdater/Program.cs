using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Factories;
using HueUpdater.Models;
using HueUpdater.Services;
using HueUpdater.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
                    var appSettings = hostContext.Configuration.Get<AppSettings>().HueUpdater;

                    var serializerSettings = NewtonsoftJsonSerializerSettingsFactory.Build();

                    services.AddSingleton(sp => appSettings.StatusUrls);
                    services.AddSingleton<IResolver<CIActivityStatus[], CIActivityStatus>, CIActivityStatusReducer>();
                    services.AddSingleton<IResolver<CIBuildStatus[], CIBuildStatus>, CIBuildStatusReducer>();
                    services.AddSingleton<IResolver<CIStatus[], CIStatus>, CIStatusReducer>();

                    services.AddSingleton<IResolver<LightStatusChangeQuery, HueAlert>, HueAlertResolver>();
                    services.AddSingleton<ISerializer<LightStatus>>(sp => CreateInstance<NewtonsoftJsonFileSerializer<LightStatus>>(sp, appSettings.Persistence.LightStatusFilePath, serializerSettings));
                    services.AddSingleton<IResolver<CIStatus, LightColor>, LightColorResolver>();

                    services.AddSingleton(sp =>
                    {
                        var hueColorFactories = new Dictionary<string, IResolver<LightColor, HueColor>>();
                        appSettings.AppearancePresets.ToList().ForEach(ap => hueColorFactories.Add(ap.Key, CreateInstance<HueColorFactory>(sp, ap.Value)));
                        return hueColorFactories;
                    });

                    services.AddSingleton(sp => appSettings.HueLights.Select(hl =>
                    {
                        var hueColorFactories = sp.GetRequiredService<Dictionary<string, IResolver<LightColor, HueColor>>>();
                        var hueColorFactory = hueColorFactories.ContainsKey(hl.AppearancePreset) ? hueColorFactories[hl.AppearancePreset] : hueColorFactories.Values.First();
                        return new HueUpdaterItem(CreateInstance<HueInvoker>(sp, hl.Endpoint), hueColorFactory);
                    }));

                    services.AddSingleton<IResolver<ScheduleQuery, bool>, ScheduleApplicabilityResolver>();
                    services.AddSingleton<IResolver<DateTime, string>>(sp => CreateInstance<ScheduleNameByCalendarResolver>(sp, appSettings.WorkPlan.Calendar));
                    services.AddSingleton<IResolver<DateTime, string>>(sp => CreateInstance<ScheduleNameByOverridesResolver>(sp, appSettings.WorkPlan.Overrides));
                    services.AddSingleton<IResolver<string[], string>>(sp => CreateInstance<ScheduleNameResolver>(sp, appSettings.WorkPlan.Schedules));
                    services.AddSingleton<IResolver<DateTime, (string ScheduleName, ScheduleSettings Schedule)>>(sp => CreateInstance<ScheduleResolver>(sp, appSettings.WorkPlan.Schedules));

                    FlurlHttp.Configure(s => s.JsonSerializer = new NewtonsoftJsonSerializer(serializerSettings));

                    services.AddHostedService<HueUpdaterService>();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConsole();
                })
                .UseConsoleLifetime();
        }

    }

}
