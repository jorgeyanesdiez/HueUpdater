using HueUpdater.Dtos;
using HueUpdater.Settings;
using System;

namespace HueUpdater.Factories
{

    /// <summary>
    /// Factory to create common instances of <see cref="HueColor"/>.
    /// </summary>
    public class HueColorFactory
    {

        /// <summary>
        /// The preset to get the color values from.
        /// </summary>
        private AppearancePresetSettings AppearancePresetSettings { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="appearancePresetSettings">The value for the <see cref="AppearancePresetSettings"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public HueColorFactory(AppearancePresetSettings appearancePresetSettings)
        {
            AppearancePresetSettings = appearancePresetSettings ?? throw new ArgumentNullException(nameof(appearancePresetSettings));
        }


        /// <summary>
        /// Factory method to create a blue color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public HueColor CreateBlue()
        {
            return new HueColor
            {
                Sat = AppearancePresetSettings.Sat,
                Bri = AppearancePresetSettings.Bri,
                Hue = AppearancePresetSettings.BlueHue
            };
        }


        /// <summary>
        /// Factory method to create a green color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public HueColor CreateGreen()
        {
            return new HueColor
            {
                Sat = AppearancePresetSettings.Sat,
                Bri = AppearancePresetSettings.Bri,
                Hue = AppearancePresetSettings.GreenHue
            };
        }


        /// <summary>
        /// Factory method to create a red color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public HueColor CreateRed()
        {
            return new HueColor
            {
                Sat = AppearancePresetSettings.Sat,
                Bri = AppearancePresetSettings.Bri,
                Hue = AppearancePresetSettings.RedHue
            };
        }


        /// <summary>
        /// Factory method to create a yellow color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public HueColor CreateYellow()
        {
            return new HueColor
            {
                Sat = AppearancePresetSettings.Sat,
                Bri = AppearancePresetSettings.Bri,
                Hue = AppearancePresetSettings.YellowHue
            };
        }

    }

}
