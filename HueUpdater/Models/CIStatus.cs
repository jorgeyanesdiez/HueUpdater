using System;

namespace HueUpdater.Models
{

    /// <summary>
    /// Models the status of a CI system.
    /// </summary>
    public sealed class CIStatus
        : IEquatable<CIStatus>
    {

        /// <summary>
        /// The activity status of the system.
        /// </summary>
        public CIActivityStatus ActivityStatus { get; set; }


        /// <summary>
        /// The build status of the system.
        /// </summary>
        public CIBuildStatus BuildStatus { get; set; }


        /// <inheritdoc/>
        public bool Equals(CIStatus other)
        {
            var result = other != null
                && ActivityStatus == other.ActivityStatus
                && BuildStatus == other.BuildStatus;

            return result;
        }

    }

}
