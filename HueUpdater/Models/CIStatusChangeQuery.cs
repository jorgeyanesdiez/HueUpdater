namespace HueUpdater.Models
{

    /// <summary>
    /// Model to query whether the status of the CI system has changed.
    /// </summary>
    public class CIStatusChangeQuery
    {

        /// <summary>
        /// The previous status of the CI system.
        /// </summary>
        public CIStatus Previous { get; set; }


        /// <summary>
        /// The current status of the CI system.
        /// </summary>
        public CIStatus Current { get; set; }

    }

}
