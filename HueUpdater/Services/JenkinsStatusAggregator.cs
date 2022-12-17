using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;
using HueUpdater.Models;

namespace HueUpdater.Services
{

    /// <summary>
    /// Queries a Jenkins endpoint to determine various status aggregates.
    /// </summary>
    /// <seealso href="https://github.com/jenkinsci/jenkins/blob/master/core/src/main/java/hudson/model/BallColor.java"/>
    public class JenkinsStatusAggregator
        : IActivityStatusProvider<Task<CIActivityStatus>>
        , IBuildStatusProvider<Task<CIBuildStatus>>
    {

        /// <summary>
        /// The base endpoint of the instance to query.
        /// </summary>
        private Uri BaseEndpoint { get; }


        /// <summary>
        /// Optional regex filter to apply on the jobs.
        /// </summary>
        private string JobNameRegexFilter { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="baseEndpoint">The value for the <see cref="BaseEndpoint"/> property.</param>
        /// <param name="jobNameRegexFilter">Optional value for the <see cref="JobNameRegexFilter"/> property.</param>
        public JenkinsStatusAggregator(string baseEndpoint, string jobNameRegexFilter = null)
        {
            BaseEndpoint = new Uri(baseEndpoint);
            JobNameRegexFilter = jobNameRegexFilter;
        }


        /// <inheritdoc/>
        public async Task<CIActivityStatus> GetActivityStatus()
        {
            var colors = await GetJobColorsAsync();
            var status = ResolveActivityStatus(colors.ToArray());
            return status;
        }


        /// <inheritdoc/>
        public async Task<CIBuildStatus> GetBuildStatus()
        {
            var colors = await GetJobColorsAsync();
            var status = ResolveBuildStatus(colors.ToArray());
            return status;
        }


        /// <summary>
        /// Queries the endpoint to get the colors for the builds.
        /// </summary>
        /// <returns>The collection of colors.</returns>
        public async Task<IEnumerable<string>> GetJobColorsAsync()
        {
            var endpoint = new Uri(BaseEndpoint, "/api/json").ToString();
            var response = await endpoint.GetJsonAsync<JenkinsOverview>();

            var jobs = string.IsNullOrWhiteSpace(JobNameRegexFilter)
                ? response.Jobs
                : response.Jobs.Where(j => Regex.IsMatch(j.Name, JobNameRegexFilter));

            var colors = jobs.Select(j => j.Color).Where(c => !Regex.IsMatch(c, "^(grey|disabled|aborted|notbuilt)"));
            return colors;
        }


        /// <summary>
        /// Resolves the activity status value from multiple individual colors.
        /// </summary>
        /// <param name="colors">The individual colors.</param>
        /// <returns>The resolved result.</returns>
        public CIActivityStatus ResolveActivityStatus(params string[] colors)
        {
            var status = colors.Any(c => c.EndsWith("anime")) ? CIActivityStatus.Building : CIActivityStatus.Idle;
            return status;
        }


        /// <summary>
        /// Resolves the build status value from multiple individual colors.
        /// </summary>
        /// <param name="colors">The individual colors.</param>
        /// <returns>The resolved result.</returns>
        public CIBuildStatus ResolveBuildStatus(params string[] colors)
        {
            var status = colors.Any(c => !c.StartsWith("blue")) ? CIBuildStatus.Broken : CIBuildStatus.Stable;
            return status;
        }

    }

}
