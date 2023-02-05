using System.Collections.Generic;

namespace HueUpdater.Settings
{

    /// <summary>
    /// Settings for the application.
    /// </summary>
    public class HueUpdaterSettings
    {
        public Dictionary<string, AppearancePresetSettings> AppearancePresets { get; set; } = new Dictionary<string, AppearancePresetSettings>();
        public List<string> StatusUrls { get; set; } = new List<string>();
        public List<HueLightSettings> HueLights { get; set; } = new List<HueLightSettings>();
        public PersistenceSettings Persistence { get; set; } = new PersistenceSettings();
        public WorkPlanSettings WorkPlan { get; set; } = new WorkPlanSettings();
    }

}
