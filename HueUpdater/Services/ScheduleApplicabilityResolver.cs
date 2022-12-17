using System;
using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Models;
using HueUpdater.Settings;

namespace HueUpdater.Services
{

    /// <summary>
    /// Determines whether a given query matches an existing schedule.
    /// </summary>
    public class ScheduleApplicabilityResolver
        : IResolver<ScheduleQuery, bool>
    {

        /// <summary>
        /// The schedules to be tested for applicability.
        /// </summary>
        private ScheduleSettings Schedules { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="schedules">The value for the <see cref="Schedules"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public ScheduleApplicabilityResolver(ScheduleSettings schedules)
        {
            Schedules = schedules ?? throw new ArgumentNullException(nameof(schedules));
        }


        /// <inheritdoc/>
        public bool Resolve(ScheduleQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            var timeSettings = Schedules.FirstOrDefault(s => s.Key == query.ScheduleName).Value;
            var isStartTimeParsed = TimeSpan.TryParse(timeSettings?.Start, out var startTime);
            var isFinishTimeParsed = TimeSpan.TryParse(timeSettings?.Finish, out var finishTime);
            var result = isStartTimeParsed && isFinishTimeParsed &&
                query.Time >= startTime && query.Time < finishTime;

            return result;
        }

    }

}
