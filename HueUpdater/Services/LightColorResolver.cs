using System;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Models;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves the color that corresponds to the status of a CI system.
    /// </summary>
    public class LightColorResolver
        : IResolver<CIStatus, LightColor>
    {

        /// <inheritdoc/>
        /// <exception cref="ArgumentNullException">If a required argument is not provided.</exception>
        /// <exception cref="NotSupportedException">If the provided argument is not supported.</exception>
        public LightColor Resolve(CIStatus input)
        {
            if (input == null) { throw new ArgumentNullException(nameof(input)); }

            var color = (input.BuildStatus, input.ActivityStatus) switch
            {
                (CIBuildStatus.Stable, CIActivityStatus.Idle) => LightColor.Green,
                (CIBuildStatus.Stable, CIActivityStatus.Building) => LightColor.Blue,
                (CIBuildStatus.Broken, CIActivityStatus.Idle) => LightColor.Red,
                (CIBuildStatus.Broken, CIActivityStatus.Building) => LightColor.Yellow,
                _ => throw new NotSupportedException(nameof(input))
            };

            return color;
        }

    }

}
