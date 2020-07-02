namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings for the application.
    /// </summary>
    public class AppSettings
    {
        public PersistenceSettings Persistence { get; set; } = new PersistenceSettings();
        public HueSettings Hue { get; set; } = new HueSettings();
        public JenkinsSettings Jenkins { get; set; } = new JenkinsSettings();
        public OperationSettings Operation { get; set; } = new OperationSettings();
    }

}
