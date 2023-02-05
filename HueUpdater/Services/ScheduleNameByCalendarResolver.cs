using System;
using System.Collections.Generic;
using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Settings;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a schedule name based on a calendar and a date.
    /// </summary>
    public class ScheduleNameByCalendarResolver
        : IResolver<DateTime, string>
    {

        /// <summary>
        /// The calendar required to resolve the schedule name.
        /// </summary>
        private Dictionary<string, DateRangeSettings[]> Calendar { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="calendar">The value for the <see cref="Calendar"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public ScheduleNameByCalendarResolver(Dictionary<string, DateRangeSettings[]> calendar)
        {
            Calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }


        /// <inheritdoc/>
        public string Resolve(DateTime date)
        {
            var scheduleName = Calendar.FirstOrDefault(dateRanges =>
                dateRanges.Value.Any(dateRange =>
                    date.Date >= dateRange.StartDate && date.Date <= dateRange.FinishDate
                )
            ).Key;

            return scheduleName;
        }

    }

}
