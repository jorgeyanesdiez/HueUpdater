namespace HueUpdater.Abstractions
{

    /// <summary>
    /// Contract for serialization services.
    /// </summary>
    public interface ISerializer
    {

        /// <summary>
        /// Reads a previously serialized object.
        /// </summary>
        /// <typeparam name="TOutput">The type of the object to read.</typeparam>
        /// <returns>The deserialized instance.</returns>
        TOutput Deserialize<TOutput>() where TOutput : new();


        /// <summary>
        /// Writes the given instance.
        /// </summary>
        /// <typeparam name="TInput">The type of the object to write.</typeparam>
        /// <param name="input">The instance to write.</param>
        void Serialize<TInput>(TInput input) where TInput : new();

    }

}
