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
        : IResolver<LightStatusChangeQuery, HueAlert>
    {

        /// <inheritdoc/>
        public HueAlert Resolve(LightStatusChangeQuery input)
        {
            var isEqual = input?.Current?.Equals(input?.Previous) ?? false;
            var alert = !isEqual
                ? HueAlertFactory.CreateBlink()
                : null;

            return alert;
        }

    }

}
