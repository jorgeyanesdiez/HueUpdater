using HueUpdater.Dtos;

namespace HueUpdater.Factories
{

    /// <summary>
    /// Factory to create common instances of <see cref="HueAlert"/>.
    /// </summary>
    public static class HueAlertFactory
    {

        /// <summary>
        /// Value for a no-alert.
        /// </summary>
        public static readonly string None = "none";


        /// <summary>
        /// Value for a blink alert.
        /// </summary>
        public static readonly string Blink = "lselect";


        /// <summary>
        /// Factory method to create an no-alert DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public static HueAlert CreateNone()
        {
            return new HueAlert { Alert = None };
        }


        /// <summary>
        /// Factory method to create a blink alert DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public static HueAlert CreateBlink()
        {
            return new HueAlert { Alert = Blink };
        }

    }

}
