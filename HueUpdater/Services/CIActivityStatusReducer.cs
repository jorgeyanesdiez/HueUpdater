using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a <see cref="CIActivityStatus"/> from multiple values.
    /// </summary>
    public class CIActivityStatusReducer
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
