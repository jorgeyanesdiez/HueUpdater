namespace HueUpdater.Abstractions
{

    /// <summary>
    /// Contract for serialization services.
    /// </summary>
    /// <typeparam name="TObject">The type of the object to work with.</typeparam>
    public interface ISerializer<TObject>
        where TObject : new()
    {

        /// <summary>
        /// Reads a previously serialized object.
        /// </summary>
        /// <returns>The deserialized object.</returns>
        TObject Deserialize();


        /// <summary>
        /// Writes the given object.
        /// </summary>
        /// <param name="input">The object to write.</param>
        void Serialize(TObject input);

    }

}
