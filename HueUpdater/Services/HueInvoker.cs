using System;
using System.Threading.Tasks;
using Flurl.Http;
using HueUpdater.Abstractions;

namespace HueUpdater.Services
{

    /// <summary>
    /// Implementation of a Hue API invoker.
    /// </summary>
    public class HueInvoker
        : IHueInvoker
    {

        /// <summary>
        /// The endpoint on which the API calls are made.
        /// </summary>
        private string Endpoint { get; }


        /// <summary>
        /// Main constructor.
        /// </summary>
        /// <param name="endpoint">The value for the <see cref="Endpoint"/> property.</param>
        public HueInvoker(string endpoint)
        {
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));
        }


        /// <inheritdoc/>
        public async Task<dynamic> PutAsync<TContent>(TContent content)
        {
            var response = await Endpoint.PutJsonAsync(content).ReceiveJsonList();
            return response;
        }

    }

}
