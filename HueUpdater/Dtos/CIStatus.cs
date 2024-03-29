﻿namespace HueUpdater.Dtos
{

    /// <summary>
    /// Incoming DTO for the data generated by
    /// <see href="https://github.com/jorgeyanesdiez/CIStatusAggregator/">CIStatusAggregator</see>.
    /// </summary>
    public class CIStatus
    {

        /// <summary>
        /// The activity status of the CI system.
        /// </summary>
        public CIActivityStatus ActivityStatus { get; set; }


        /// <summary>
        /// The build status of the CI system.
        /// </summary>
        public CIBuildStatus BuildStatus { get; set; }

    }

}
