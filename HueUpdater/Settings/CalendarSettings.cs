namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings that represent a calendar.
    /// </summary>
    public class CalendarSettings
    {
        public CalendarDefaultSettings Defaults { get; set; } = new CalendarDefaultSettings();
        public CalendarDayOverrideSettings DayOverrides { get; set; } = new CalendarDayOverrideSettings();
        public CalendarDayOverrideExclusionSettings DayOverridesExclusions { get; set; } = new CalendarDayOverrideExclusionSettings();
    }

}
