namespace HueUpdater.Dtos
{

    /// <summary>
    /// Outgoing DTO for a light color in the Hue API.
    /// </summary>
    public class HueColor
    {

        /// <summary>
        /// Forces the power status to be on.
        /// </summary>
        public bool On { get; } = true;


        /// <summary>
        /// The saturation of the color.
        /// </summary>
        public int Sat { get; set; } = 254;


        /// <summary>
        /// The brightness of the color.
        /// </summary>
        public int Bri { get; set; } = 50;


        /// <summary>
        /// The hue of the color.
        /// </summary>
        public int Hue { get; set; }

    }

}
