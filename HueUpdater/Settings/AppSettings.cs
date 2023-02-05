namespace HueUpdater.Settings
{

    /// <summary>
    /// Sections of the appsettings file used by this application.
    /// </summary>
    public class AppSettings
    {
        public HueUpdaterSettings HueUpdater { get; set; } = new HueUpdaterSettings();
    }

}
