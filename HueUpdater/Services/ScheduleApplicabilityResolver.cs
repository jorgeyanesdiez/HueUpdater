using System;
using System.Collections.Generic;
using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Models;
using HueUpdater.Settings;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves the applicability of a schedule based on a schedule and a time.
    /// </summary>
    public class ScheduleApplicabilityResolver
        : IResolver<ScheduleQuery, bool>
    {

        /// <summary>
        /// The schedule required to determine the applicability.
        /// </summary>
        private IDictionary<string, TimeRangeSettings> Schedule { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="schedule">The value for the <see cref="Schedule"/> property.</param>
        public ScheduleApplicabilityResolver(IDictionary<string, TimeRangeSettings> schedule)
        {
            Schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
        }


        /// <inheritdoc/>
        public bool Resolve(ScheduleQuery query)
        {
            var timeSettings = Schedule.FirstOrDefault(s => s.Key == query.ScheduleName).Value;
            var isStartTimeParsed = TimeSpan.TryParse(timeSettings?.Start, out var startTime);
            var isFinishTimeParsed = TimeSpan.TryParse(timeSettings?.Finish, out var finishTime);
            var result = isStartTimeParsed && isFinishTimeParsed &&
                query.Time >= startTime && query.Time < finishTime;

            return result;
        }

    }

}
