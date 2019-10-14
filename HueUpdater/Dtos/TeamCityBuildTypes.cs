using System.Collections.Generic;
using System.Linq;

namespace HueUpdater.Dtos
{

    /// <summary>
    /// Incoming DTO for the build types of a TeamCity instance.
    /// </summary>
    public class TeamCityBuildTypes
    {
        public IEnumerable<TeamCityBuildType> BuildType { get; set; } = Enumerable.Empty<TeamCityBuildType>();
    }

}
