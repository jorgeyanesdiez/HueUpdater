namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings for date and time services.
    /// </summary>
    public class OperationSettings
    {
        public ScheduleSettings Schedules { get; set; } = new ScheduleSettings();
        public CalendarSettings Calendar { get; set; } = new CalendarSettings();
    }

}
