using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Factories;
using HueUpdater.Models;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves the alert that corresponds to the status of a CI system.
    /// </summary>
    public class HueAlertResolver
        : IResolver<CIStatusChangeQuery, HueAlert>
    {

        /// <inheritdoc/>
        public HueAlert Resolve(CIStatusChangeQuery input)
        {
            var isEqual = input?.Current?.Equals(input?.Previous) ?? false;
            var alert = isEqual
                ? HueAlertFactory.CreateNone()
                : HueAlertFactory.CreateBlink();

            return alert;
        }

    }

}
