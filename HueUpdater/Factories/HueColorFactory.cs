using HueUpdater.Dtos;
using HueUpdater.Settings;
using System;

namespace HueUpdater.Factories
{

    /// <summary>
    /// Default implementation of <see cref="IHueColorFactory"/>.
    /// </summary>
    public class HueColorFactory : IHueColorFactory
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


        /// <inheritdoc/>
        public HueColor CreateBlue()
        {
            return new HueColor
            {
                Sat = AppearancePresetSettings.Sat,
                Bri = AppearancePresetSettings.Bri,
                Hue = AppearancePresetSettings.BlueHue
            };
        }


        /// <inheritdoc/>
        public HueColor CreateGreen()
        {
            return new HueColor
            {
                Sat = AppearancePresetSettings.Sat,
                Bri = AppearancePresetSettings.Bri,
                Hue = AppearancePresetSettings.GreenHue
            };
        }


        /// <inheritdoc/>
        public HueColor CreateRed()
        {
            return new HueColor
            {
                Sat = AppearancePresetSettings.Sat,
                Bri = AppearancePresetSettings.Bri,
                Hue = AppearancePresetSettings.RedHue
            };
        }


        /// <inheritdoc/>
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
