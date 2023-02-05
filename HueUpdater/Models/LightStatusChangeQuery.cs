namespace HueUpdater.Models
{

    /// <summary>
    /// Query to determine whether the status of a light has changed.
    /// </summary>
    public class LightStatusChangeQuery
    {

        /// <summary>
        /// The previous status of the light.
        /// </summary>
        public LightStatus Previous { get; set; }


        /// <summary>
        /// The current status of the light.
        /// </summary>
        public LightStatus Current { get; set; }

    }

}
