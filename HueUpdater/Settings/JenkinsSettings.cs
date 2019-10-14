namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings for Jenkins services.
    /// </summary>
    public class JenkinsSettings
    {
        public string BaseEndpoint { get; set; }
        public string JobNameRegexFilter { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

}
