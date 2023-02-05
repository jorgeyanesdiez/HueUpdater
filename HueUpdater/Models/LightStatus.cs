using System;

namespace HueUpdater.Models
{

    /// <summary>
    /// Model to represent the status of a light.
    /// </summary>
    public sealed class LightStatus
        : IEquatable<LightStatus>
    {

        /// <summary>
        /// The power status of the light.
        /// </summary>
        public LightPower Power { get; set; }


        /// <summary>
        /// The color of the light.
        /// </summary>
        public LightColor? Color { get; set; }


        /// <inheritdoc/>
        public bool Equals(LightStatus other)
        {
            var result = other != null
                && Power == other.Power
                && Color == other.Color;

            return result;
        }


        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as LightStatus);
        }


        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Power, Color);
        }

    }

}
