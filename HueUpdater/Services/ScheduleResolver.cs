using System;
using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Settings;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a schedule based on a date.
    /// </summary>
    public class ScheduleResolver
        : IResolver<DateTime, (string Name, TimeRangeSettings Times)>
    {

        /// <summary>
        /// The service required to get the name of a schedule.
        /// </summary>
        private IResolver<DateTime, string> ScheduleNameResolver { get; }


        /// <summary>
        /// The available schedules.
        /// </summary>
        private ScheduleSettings Schedules { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="scheduleNameResolver">The value for the <see cref="ScheduleNameResolver"/> property.</param>
        /// <param name="schedules">The value for the <see cref="Schedules"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If a required dependency is not valid.</exception>
        public ScheduleResolver(
            IResolver<DateTime, string> scheduleNameResolver,
            ScheduleSettings schedules)
        {
            ScheduleNameResolver = scheduleNameResolver ?? throw new ArgumentNullException(nameof(scheduleNameResolver));
            Schedules = schedules ?? throw new ArgumentNullException(nameof(schedules));
            if (!schedules.Keys.Any()) { throw new ArgumentOutOfRangeException(nameof(schedules)); }
        }


        /// <inheritdoc/>
        public (string Name, TimeRangeSettings Times) Resolve(DateTime input)
        {
            var scheduleName = ScheduleNameResolver.Resolve(input.Date) ?? Schedules.Keys.First();
            var schedule = Schedules[scheduleName];
            return (scheduleName, schedule);
        }

    }

}
