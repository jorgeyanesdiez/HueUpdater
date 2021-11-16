using System;
using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Settings;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a schedule name based on a calendar and a date.
    /// </summary>
    public class ScheduleNameResolver
        : IResolver<DateTime, string>
    {

        /// <summary>
        /// The calendar required to resolve the schedule name.
        /// </summary>
        private CalendarSettings Calendar { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="calendar">The value for the <see cref="Calendar"/> property.</param>
        public ScheduleNameResolver(CalendarSettings calendar)
        {
            Calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }


        /// <inheritdoc/>
        public string Resolve(DateTime date)
        {
            var overridenName = ResolveOverriden(date);
            var defaultName = ResolveDefault(date);
            var scheduleName = defaultName;

            if (overridenName != null && !(Calendar.DayOverridesExclusions?.Contains(defaultName ?? Guid.NewGuid().ToString()) ?? false))
            {
                scheduleName = overridenName;
            }

            return scheduleName;
        }


        /// <summary>
        /// Resolves a schedule name according to the day override settings.
        /// </summary>
        /// <param name="date">The date to find a schedule name for.</param>
        /// <returns>The name of the schedule.</returns>
        public string ResolveOverriden(DateTime date)
        {
            var scheduleName = Calendar.DayOverrides?.OrderBy(grp => grp.Key).FirstOrDefault(dayNames =>
                dayNames.Value.Any(dayName =>
                {
                    var isDayParsed = Enum.TryParse<DayOfWeek>(dayName, out var dayOfWeek);
                    var result = isDayParsed && date.Date.DayOfWeek == dayOfWeek;
                    return result;
                })
            ).Key;

            return scheduleName;
        }


        /// <summary>
        /// Resolves a schedule name according to the default calendar settings.
        /// </summary>
        /// <param name="date">The date to find a schedule name for.</param>
        /// <returns>The name of the schedule.</returns>
        public string ResolveDefault(DateTime date)
        {
            var scheduleName = Calendar.Defaults?.OrderBy(grp => grp.Key).FirstOrDefault(dateRanges =>
                dateRanges.Value.Any(dateRange =>
                {
                    var isStartDateParsed = DateTime.TryParse(dateRange.Start, out var startDate);
                    var isFinishDateParsed = DateTime.TryParse(dateRange.Finish, out var finishDate);
                    var result = isStartDateParsed && isFinishDateParsed &&
                        date.Date >= startDate && date.Date <= finishDate;
                    return result;
                })
            ).Key;

            return scheduleName;
        }

    }

}
