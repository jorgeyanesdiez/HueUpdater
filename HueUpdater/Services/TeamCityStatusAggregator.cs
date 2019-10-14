using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Models;

namespace HueUpdater.Services
{

    /// <summary>
    /// Queries a TeamCity endpoint to determine various status aggregates.
    /// </summary>
    public class TeamCityStatusAggregator
        : IActivityStatusAggregator<Task<CIActivityStatus>>
        , IBuildStatusAggregator<Task<CIBuildStatus>>
    {

        /// <summary>
        /// The base endpoint of the instance to query.
        /// </summary>
        private Uri BaseEndpoint { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="baseEndpoint">The value for the <see cref="BaseEndpoint"/> property.</param>
        public TeamCityStatusAggregator(string baseEndpoint)
        {
            BaseEndpoint = new Uri(baseEndpoint);
        }


        /// <inheritdoc/>
        public async Task<CIActivityStatus> GetActivityStatus()
        {
            var count = await GetRunningBuildCountAsync();
            var status = ResolveActivityStatus(count);
            return status;
        }


        /// <inheritdoc/>
        public async Task<CIBuildStatus> GetBuildStatus()
        {
            var hrefs = await GetBuildHrefsAsync();
            var statuses = await GetBuildStatusesAsync(hrefs.ToArray());
            var status = ResolveBuildStatus(statuses.ToArray());
            return status;
        }


        /// <summary>
        /// Queries the endpoint to get the number of running builds.
        /// </summary>
        /// <returns>The count of running builds.</returns>
        public async Task<int> GetRunningBuildCountAsync()
        {
            var endpoint = new Uri(BaseEndpoint, "/app/rest/builds/?locator=running:true").ToString();
            var response = await endpoint.GetJsonAsync<TeamCityBuilds>();
            return response.Count;
        }


        /// <summary>
        /// Queries the endpoint to get the href values of the build types.
        /// </summary>
        /// <returns>The collection of hrefs.</returns>
        public async Task<IEnumerable<string>> GetBuildHrefsAsync()
        {
            var endpoint = new Uri(BaseEndpoint, "/app/rest/buildTypes/").ToString();
            var response = await endpoint.GetJsonAsync<TeamCityBuildTypes>();
            var hrefs = response.BuildType.Select(bt => bt.Href);
            return hrefs;
        }


        /// <summary>
        /// Queries the endpoint on the given hrefs to determine the status of the builds.
        /// </summary>
        /// <param name="hrefs">The hrefs of the builds to query.</param>
        /// <returns>The collection of build status values.</returns>
        public async Task<IEnumerable<string>> GetBuildStatusesAsync(params string[] hrefs)
        {
            var tasks = new List<Task<TeamCityBuilds>>();
            foreach (var href in hrefs ?? Enumerable.Empty<string>())
            {
                var endpoint = new Uri(BaseEndpoint, $"{href}/builds/?count=1").ToString();
                tasks.Add(endpoint.GetJsonAsync<TeamCityBuilds>());
            }
            var builds = await Task.WhenAll(tasks);
            var statuses = builds.SelectMany(b => b.Build).Select(s => s.Status);
            return statuses;
        }


        /// <summary>
        /// Resolves the activity status from the given running build count.
        /// </summary>
        /// <param name="count">The count of currently running builds.</param>
        /// <returns>The resolved result.</returns>
        public CIActivityStatus ResolveActivityStatus(int count)
        {
            var status = count > 0 ? CIActivityStatus.Building : CIActivityStatus.Idle;
            return status;
        }


        /// <summary>
        /// Resolves the build status from all the individual build status values.
        /// </summary>
        /// <param name="statuses">The individual build status values.</param>
        /// <returns>The resolved result.</returns>
        public CIBuildStatus ResolveBuildStatus(params string[] statuses)
        {
            var status = statuses.Any(c => c != "SUCCESS") ? CIBuildStatus.Broken : CIBuildStatus.Stable;
            return status;
        }

    }

}
