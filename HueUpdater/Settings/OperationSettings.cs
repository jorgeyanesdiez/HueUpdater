using System.Collections.Generic;

namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings for date and time services.
    /// </summary>
    public class OperationSettings
    {
        public IDictionary<string, TimeRangeSettings> Schedule { get; set; } = new Dictionary<string, TimeRangeSettings>();
        public CalendarSettings Calendar { get; set; } = new CalendarSettings();
    }

}
