namespace HueUpdater.Abstractions
{

    /// <summary>
    /// Contract for value resolvers.
    /// </summary>
    /// <typeparam name="TInput">The type of the input supported.</typeparam>
    /// <typeparam name="TResult">The type of the resolved result.</typeparam>
    public interface IResolver<in TInput, out TResult>
    {

        /// <summary>
        /// Resolves a value from the given input.
        /// </summary>
        /// <param name="input">The input for the resolution operation.</param>
        /// <returns>The resolved result.</returns>
        TResult Resolve(TInput input);

    }

}
