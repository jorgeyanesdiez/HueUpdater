using System.Collections.Generic;
using System.Linq;

namespace HueUpdater.Dtos
{

    /// <summary>
    /// Incoming DTO for the builds of a TeamCity instance.
    /// </summary>
    public class TeamCityBuilds
    {
        public int Count { get; set; }
        public IEnumerable<TeamCityBuild> Build { get; set; } = Enumerable.Empty<TeamCityBuild>();
    }

}
