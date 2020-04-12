using System.Collections.Generic;

namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings that represent schedules.
    /// </summary>
    public class ScheduleSettings
        : Dictionary<string, TimeRangeSettings>
    {
    }

}
