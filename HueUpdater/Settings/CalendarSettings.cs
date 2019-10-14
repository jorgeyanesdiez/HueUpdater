using System.Collections.Generic;

namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings that represent a calendar.
    /// </summary>
    public class CalendarSettings
    {
        public IDictionary<string, DateRangeSettings[]> Defaults { get; set; } = new Dictionary<string, DateRangeSettings[]>();
        public IDictionary<string, string[]> DayOverrides { get; set; } = new Dictionary<string, string[]>();
    }

}
