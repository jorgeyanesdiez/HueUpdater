namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings for Jenkins services.
    /// </summary>
    public class JenkinsSettings
    {
        public string BaseEndpoint { get; set; }
        public string JobNameRegexFilter { get; set; } = null;
        public string User { get; set; } = null;
        public string Password { get; set; } = null;
    }

}
