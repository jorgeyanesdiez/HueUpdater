using System;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;

namespace HueUpdater.Models
{

    /// <summary>
    /// Model for a unit processable by a <see cref="HueUpdaterService"/> .
    /// </summary>
    public class HueUpdaterItem
    {

        /// <summary>
        /// The service used to invoke the Hue API.
        /// </summary>
        public IHueInvoker HueInvoker { get; }


        /// <summary>
        /// The service used to create a <see cref="HueColor"/> DTO from a <see cref="LightColor"/>
        /// </summary>
        public IResolver<LightColor, HueColor> HueColorFactory { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="hueInvoker">The value for the <see cref="HueInvoker"/> property.</param>
        /// <param name="hueColorFactory">The value for the <see cref="HueColorFactory"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public HueUpdaterItem(
            IHueInvoker hueInvoker,
            IResolver<LightColor, HueColor> hueColorFactory
        )
        {
            HueInvoker = hueInvoker ?? throw new ArgumentNullException(nameof(hueInvoker));
            HueColorFactory = hueColorFactory ?? throw new ArgumentNullException(nameof(hueColorFactory));
        }

    }

}
