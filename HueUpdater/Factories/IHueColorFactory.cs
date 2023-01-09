using HueUpdater.Dtos;

namespace HueUpdater.Factories
{

    /// <summary>
    /// Contract for factories that create common instances of <see cref="HueColor"/>.
    /// </summary>
    public interface IHueColorFactory
    {

        /// <summary>
        /// Factory method to create a blue color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public HueColor CreateBlue();


        /// <summary>
        /// Factory method to create a green color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public HueColor CreateGreen();


        /// <summary>
        /// Factory method to create a red color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public HueColor CreateRed();


        /// <summary>
        /// Factory method to create a yellow color DTO.
        /// </summary>
        /// <returns>The requested DTO.</returns>
        public HueColor CreateYellow();

    }

}
