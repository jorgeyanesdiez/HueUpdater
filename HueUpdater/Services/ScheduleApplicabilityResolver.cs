using System;
using HueUpdater.Abstractions;
using HueUpdater.Models;

namespace HueUpdater.Services
{

    /// <summary>
    /// Determines whether a time is considered working hours within a schedule.
    /// </summary>
    public class ScheduleApplicabilityResolver
        : IResolver<ScheduleQuery, bool>
    {

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If a required argument is not provided.</exception>
        public bool Resolve(ScheduleQuery query)
        {
            if (query == null) { throw new ArgumentNullException(nameof(query)); }

            var result = false;

            if (query.Schedule?.Hours != null)
            {
                result = query.Time >= query.Schedule.Hours.StartTime
                    && query.Time < query.Schedule.Hours.FinishTime;
            }

            return result;
        }

    }

}
