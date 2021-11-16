using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Models;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves an activity status from multiple values.
    /// </summary>
    public class CIActivityStatusResolver
        : IResolver<CIActivityStatus[], CIActivityStatus>
    {

        /// <inheritdoc />
        public CIActivityStatus Resolve(params CIActivityStatus[] input)
        {
            var statusCollection = input ?? Enumerable.Empty<CIActivityStatus>();
            var status = statusCollection.Any(s => s == CIActivityStatus.Building) ? CIActivityStatus.Building : CIActivityStatus.Idle;
            return status;
        }

    }

}
