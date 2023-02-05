using System;
using System.Collections.Generic;
using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Settings;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a schedule based on a date.
    /// </summary>
    public class ScheduleResolver
        : IResolver<DateTime, (string ScheduleName, ScheduleSettings Schedule)>
    {

        /// <summary>
        /// The services required to get candidate schedule names based on a date.
        /// </summary>
        private IEnumerable<IResolver<DateTime, string>> ScheduleNameCandidateResolvers { get; }


        /// <summary>
        /// The service required to reduce the candidate schedule names.
        /// </summary>
        private IResolver<string[], string> ScheduleNameResolver { get; }


        /// <summary>
        /// The schedules required to construct the resolved values.
        /// </summary>
        private Dictionary<string, ScheduleSettings> Schedules { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="scheduleNameCandidateResolvers">The value for the <see cref="ScheduleNameCandidateResolvers"/> property.</param>
        /// <param name="scheduleNameResolver">The value for the <see cref="ScheduleNameResolver"/> property.</param>
        /// <param name="schedules">The value for the <see cref="Schedules"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public ScheduleResolver(
            IEnumerable<IResolver<DateTime, string>> scheduleNameCandidateResolvers,
            IResolver<string[], string> scheduleNameResolver,
            Dictionary<string, ScheduleSettings> schedules
        )
        {
            ScheduleNameCandidateResolvers = scheduleNameCandidateResolvers ?? throw new ArgumentNullException(nameof(scheduleNameCandidateResolvers));
            ScheduleNameResolver = scheduleNameResolver ?? throw new ArgumentNullException(nameof(scheduleNameResolver));
            Schedules = schedules ?? throw new ArgumentNullException(nameof(schedules));
        }


        /// <inheritdoc/>
        public (string ScheduleName, ScheduleSettings Schedule) Resolve(DateTime input)
        {
            var scheduleCandidateNames = ScheduleNameCandidateResolvers.Select(scr => scr.Resolve(input)).ToArray();
            var scheduleName = ScheduleNameResolver.Resolve(scheduleCandidateNames);
            var schedule = Schedules[scheduleName];
            return (scheduleName, schedule);
        }

    }

}
