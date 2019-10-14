namespace HueUpdater.Dtos
{

    /// <summary>
    /// Outgoing DTO for a light power status in the Hue API.
    /// </summary>
    public class HuePower
    {

        /// <summary>
        /// The power status.
        /// </summary>
        public bool On { get; set; }

    }

}
