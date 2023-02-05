using System;
using System.Linq;
using HueUpdater.Abstractions;
using HueUpdater.Dtos;

namespace HueUpdater.Services
{

    /// <summary>
    /// Resolves a <see cref="CIStatus"/> from multiple values.
    /// </summary>
    public class CIStatusReducer
        : IResolver<CIStatus[], CIStatus>
    {

        /// <summary>
        /// The service required to reduce the <see cref="CIActivityStatus"/> values.
        /// </summary>
        private IResolver<CIActivityStatus[], CIActivityStatus> ActivityStatusReducer { get; }


        /// <summary>
        /// The service required to reduce the <see cref="CIBuildStatus"/> values.
        /// </summary>
        private IResolver<CIBuildStatus[], CIBuildStatus> BuildStatusReducer { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="activityStatusReducer">The value for the <see cref="ActivityStatusReducer"/> property.</param>
        /// <param name="buildStatusReducer">The value for the <see cref="BuildStatusReducer"/> property.</param>
        /// <exception cref="ArgumentNullException">If a required dependency is not provided.</exception>
        public CIStatusReducer(
            IResolver<CIActivityStatus[], CIActivityStatus> activityStatusReducer,
            IResolver<CIBuildStatus[], CIBuildStatus> buildStatusReducer
        )
        {
            ActivityStatusReducer = activityStatusReducer ?? throw new ArgumentNullException(nameof(activityStatusReducer));
            BuildStatusReducer = buildStatusReducer ?? throw new ArgumentNullException(nameof(buildStatusReducer));
        }


        /// <inheritdoc />
        public CIStatus Resolve(params CIStatus[] input)
        {
            var status = new CIStatus()
            {
                ActivityStatus = ActivityStatusReducer.Resolve(input.Select(s => s.ActivityStatus).ToArray()),
                BuildStatus = BuildStatusReducer.Resolve(input.Select(s => s.BuildStatus).ToArray())
            };
            return status;
        }

    }

}
