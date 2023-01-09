using System;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Factories;
using HueUpdater.Models;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves the color that corresponds to the status of a CI system.
    /// </summary>
    public class HueColorResolver
        : IResolver<CIStatus, HueColor>
    {

        /// <summary>
        /// The factory to get the colors from.
        /// </summary>
        private IHueColorFactory HueColorFactory { get; set; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="hueColorFactory">The value for the <see cref="HueColorFactory"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public HueColorResolver(IHueColorFactory hueColorFactory)
        {
            HueColorFactory = hueColorFactory ?? throw new ArgumentNullException(nameof(hueColorFactory));
        }


        /// <inheritdoc/>
        public HueColor Resolve(CIStatus input)
        {
            if (input == null) { throw new ArgumentNullException(nameof(input)); }

            HueColor color;

            if (input.ActivityStatus == CIActivityStatus.Idle)
            {
                if (input.BuildStatus == CIBuildStatus.Stable) { color = HueColorFactory.CreateGreen(); }
                else { color = HueColorFactory.CreateRed(); }
            }
            else
            {
                if (input.BuildStatus == CIBuildStatus.Stable) { color = HueColorFactory.CreateBlue(); }
                else { color = HueColorFactory.CreateYellow(); }
            }

            return color;
        }

    }

}
