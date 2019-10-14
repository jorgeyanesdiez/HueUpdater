using HueUpdater.Dtos;

namespace HueUpdater.Factories
{

    /// <summary>
    /// Factory to create common instances of <see cref="HueColor"/>.
    /// </summary>
    public static class HueColorFactory
    {

        /// <summary>
        /// Value for a blue color hue.
        /// </summary>
        public static readonly int BlueHue = 44000;


        /// <summary>
        /// Value for a green color hue.
        /// </summary>
        public static readonly int GreenHue = 26000;


        /// <summary>
        /// Value for a red color hue.
        /// </summary>
        public static readonly int RedHue = 65000;


        /// <summary>
        /// Value for a yellow color hue.
        /// </summary>
        public static readonly int YellowHue = 10000;


        /// <summary>
        /// Factory method to create a blue color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public static HueColor CreateBlue()
        {
            return new HueColor { Hue = BlueHue };
        }


        /// <summary>
        /// Factory method to create a green color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public static HueColor CreateGreen()
        {
            return new HueColor { Hue = GreenHue };
        }


        /// <summary>
        /// Factory method to create a red color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public static HueColor CreateRed()
        {
            return new HueColor { Hue = RedHue };
        }


        /// <summary>
        /// Factory method to create a yellow color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public static HueColor CreateYellow()
        {
            return new HueColor { Hue = YellowHue };
        }

    }

}
