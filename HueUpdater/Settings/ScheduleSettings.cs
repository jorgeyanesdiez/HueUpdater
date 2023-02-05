namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings to define a schedule.
    /// </summary>
    public class ScheduleSettings
    {
        public uint Priority { get; set; }
        public TimeRangeSettings Hours { get; set; } = new TimeRangeSettings();
    }

}
