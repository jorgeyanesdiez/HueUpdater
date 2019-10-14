using System.Threading.Tasks;

namespace HueUpdater.Abstractions
{

    /// <summary>
    /// Contract for services that invoke the Hue API.
    /// </summary>
    public interface IHueInvoker
    {

        /// <summary>
        /// Invokes a PUT method of the Hue API.
        /// </summary>
        /// <typeparam name="TContent">The type of the body of the message.</typeparam>
        /// <param name="content">The body of the message.</param>
        /// <returns>The response from the Hue API.</returns>
        Task<dynamic> PutAsync<TContent>(TContent content);

    }

}
