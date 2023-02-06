using System;
using System.Collections.Generic;
using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Settings;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a schedule name based on its priority.
    /// </summary>
    public class ScheduleNameResolver
        : IResolver<string[], string>
    {

        /// <summary>
        /// The schedules required for lookups.
        /// </summary>
        private Dictionary<string, ScheduleSettings> Schedules { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="schedules">The value for the <see cref="Schedules"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public ScheduleNameResolver(Dictionary<string, ScheduleSettings> schedules)
        {
            Schedules = schedules ?? throw new ArgumentNullException(nameof(schedules));
        }


        /// <inheritdoc/>
        public string Resolve(string[] input)
        {
            var lookup = input.Where(i => i != null).Select(scheduleName => new
            {
                Name = scheduleName,
                Priority = Schedules.ContainsKey(scheduleName)
                ? Schedules[scheduleName].Priority
                : uint.MaxValue
            });

            var result = lookup.MinBy(schedule => schedule.Priority).Name;
            return result;
        }

    }

}
