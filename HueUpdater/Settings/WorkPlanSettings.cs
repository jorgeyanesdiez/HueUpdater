using System.Collections.Generic;

namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings for the work plan.
    /// </summary>
    public class WorkPlanSettings
    {
        public Dictionary<string, ScheduleSettings> Schedules { get; set; } = new Dictionary<string, ScheduleSettings>();
        public Dictionary<string, DateRangeSettings[]> Calendar { get; set; } = new Dictionary<string, DateRangeSettings[]>();
        public Dictionary<string, string> Overrides { get; set; } = new Dictionary<string, string>();
    }

}
