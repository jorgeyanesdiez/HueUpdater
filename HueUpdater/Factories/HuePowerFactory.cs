using HueUpdater.Dtos;

namespace HueUpdater.Factories
{

    /// <summary>
    /// Factory to create common instances of <see cref="HuePower"/>.
    /// </summary>
    public static class HuePowerFactory
    {

        /// <summary>
        /// Value for a powered-off state.
        /// </summary>
        public static readonly bool Off = false;


        /// <summary>
        /// Value for a powered-on state.
        /// </summary>
        public static readonly bool On = true;


        /// <summary>
        /// Factory method to create a power off DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public static HuePower CreateOff()
        {
            return new HuePower { On = Off };
        }


        /// <summary>
        /// Factory method to create a power on DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public static HuePower CreateOn()
        {
            return new HuePower { On = On };
        }

    }

}
