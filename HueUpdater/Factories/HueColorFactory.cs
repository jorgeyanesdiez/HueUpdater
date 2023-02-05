using System;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Models;
using HueUpdater.Settings;

namespace HueUpdater.Factories
{

    /// <summary>
    /// Factory to create common instances of <see cref="HueColor"/> from a <see cref="LightColor"/>.
    /// </summary>
    public class HueColorFactory
        : IResolver<LightColor, HueColor>
    {

        /// <summary>
        /// The preset to get the color values from.
        /// </summary>
        private AppearancePresetSettings AppearancePreset { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="appearancePreset">The value for the <see cref="AppearancePreset"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public HueColorFactory(AppearancePresetSettings appearancePreset)
        {
            AppearancePreset = appearancePreset ?? throw new ArgumentNullException(nameof(appearancePreset));
        }


        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">If the provided argument is not supported.</exception>
        public HueColor Resolve(LightColor input)
        {
            var result = new HueColor
            {
                Sat = AppearancePreset.Sat,
                Bri = AppearancePreset.Bri,
                Hue = input switch
                {
                    LightColor.Green => AppearancePreset.GreenHue,
                    LightColor.Blue => AppearancePreset.BlueHue,
                    LightColor.Red => AppearancePreset.RedHue,
                    LightColor.Yellow => AppearancePreset.YellowHue,
                    _ => throw new NotSupportedException(nameof(input))
                }
            };
            return result;
        }

    }

}
