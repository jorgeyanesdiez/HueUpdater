using System;
using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Settings;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a schedule based on a calendar and a date.
    /// </summary>
    public class ScheduleResolver
        : IResolver<DateTime, string>
    {

        /// <summary>
        /// The calendar required to resolve the schedule.
        /// </summary>
        private CalendarSettings Calendar { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="calendar">The value for the <see cref="Calendar"/> property.</param>
        public ScheduleResolver(CalendarSettings calendar)
        {
            Calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
        }


        /// <inheritdoc/>
        public string Resolve(DateTime date)
        {
            var schedule = ResolveDayOverridenSchedule(date) ?? ResolveDefaultSchedule(date);
            return schedule;
        }


        /// <summary>
        /// Resolves a schedule according to the day override settings.
        /// </summary>
        /// <param name="date">The date to find a schedule for.</param>
        /// <returns>The name of the schedule.</returns>
        public string ResolveDayOverridenSchedule(DateTime date)
        {
            var schedule = Calendar?.DayOverrides.OrderBy(grp => grp.Key).FirstOrDefault(dayNames =>
                dayNames.Value.Any(dayName =>
                {
                    var isDayParsed = Enum.TryParse<DayOfWeek>(dayName, out var dayOfWeek);
                    var result = isDayParsed && date.Date.DayOfWeek == dayOfWeek;
                    return result;
                })
            ).Key;

            return schedule;
        }


        /// <summary>
        /// Resolves a schedule according to the default calendar settings.
        /// </summary>
        /// <param name="date">The date to find a schedule for.</param>
        /// <returns>The name of the schedule.</returns>
        public string ResolveDefaultSchedule(DateTime date)
        {
            var schedule = Calendar?.Defaults.OrderBy(grp => grp.Key).FirstOrDefault(dateRanges =>
                dateRanges.Value.Any(dateRange =>
                {
                    var isStartDateParsed = DateTime.TryParse(dateRange.Start, out var startDate);
                    var isFinishDateParsed = DateTime.TryParse(dateRange.Finish, out var finishDate);
                    var result = isStartDateParsed && isFinishDateParsed &&
                        date.Date >= startDate && date.Date < finishDate;
                    return result;
                })
            ).Key;

            return schedule;
        }

    }

}
