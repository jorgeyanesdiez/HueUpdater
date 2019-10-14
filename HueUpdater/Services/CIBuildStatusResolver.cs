using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Models;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a build status from multiple values.
    /// </summary>
    public class CIBuildStatusResolver
        : IResolver<CIBuildStatus[], CIBuildStatus>
    {

        /// <inheritdoc />
        public CIBuildStatus Resolve(params CIBuildStatus[] input)
        {
            var statuses = input ?? Enumerable.Empty<CIBuildStatus>();
            var status = statuses.Any(s => s == CIBuildStatus.Broken) ? CIBuildStatus.Broken : CIBuildStatus.Stable;
            return status;
        }

    }

}
