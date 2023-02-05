using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a <see cref="CIBuildStatus"/> from multiple values.
    /// </summary>
    public class CIBuildStatusReducer
        : IResolver<CIBuildStatus[], CIBuildStatus>
    {

        /// <inheritdoc />
        public CIBuildStatus Resolve(params CIBuildStatus[] input)
        {
            var statusCollection = input ?? Enumerable.Empty<CIBuildStatus>();
            var status = statusCollection.Any(s => s == CIBuildStatus.Broken) ? CIBuildStatus.Broken : CIBuildStatus.Stable;
            return status;
        }

    }

}
