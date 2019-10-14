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
            var statuses = input ?? Enumerable.Empty<CIActivityStatus>();
            var status = statuses.Any(s => s == CIActivityStatus.Building) ? CIActivityStatus.Building : CIActivityStatus.Idle;
            return status;
        }

    }

}
