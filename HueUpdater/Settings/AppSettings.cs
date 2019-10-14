namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings for the application.
    /// </summary>
    public class AppSettings
    {
        public string LastStatusFilePath { get; set; }
        public HueSettings Hue { get; set; } = new HueSettings();
        public JenkinsSettings Jenkins { get; set; } = new JenkinsSettings();
        public TeamCitySettings TeamCity { get; set; } = new TeamCitySettings();
        public OperationSettings Operation { get; set; } = new OperationSettings();
    }

}
