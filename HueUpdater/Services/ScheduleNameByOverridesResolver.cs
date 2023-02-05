using System;
using System.Collections.Generic;
using System.Globalization;
using HueUpdater.Abstractions;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a schedule name based on a collection of overrides and a date.
    /// </summary>
    public class ScheduleNameByOverridesResolver
        : IResolver<DateTime, string>
    {

        /// <summary>
        /// The overrides required to resolve the schedule name.
        /// </summary>
        private Dictionary<string, string> Overrides { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="overrides">The value for the <see cref="Overrides"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public ScheduleNameByOverridesResolver(Dictionary<string, string> overrides)
        {
            Overrides = overrides ?? throw new ArgumentNullException(nameof(overrides));
        }


        /// <inheritdoc/>
        public string Resolve(DateTime date)
        {
            var culture = new CultureInfo("en-US");
            var day = culture.TextInfo.ToTitleCase(culture.DateTimeFormat.GetDayName(date.DayOfWeek));
            var scheduleName = Overrides.ContainsKey(day) ? Overrides[day] : null;
            return scheduleName;
        }

    }

}
