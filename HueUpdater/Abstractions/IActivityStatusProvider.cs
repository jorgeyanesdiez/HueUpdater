namespace HueUpdater.Abstractions
{

    /// <summary>
    /// Contract for services that provide an activity status.
    /// </summary>
    /// <typeparam name="TActivityStatus">The type of the activity status.</typeparam>
    public interface IActivityStatusProvider<out TActivityStatus>
    {

        /// <summary>
        /// Provides an activity status.
        /// </summary>
        /// <returns>The requested value.</returns>
        TActivityStatus GetActivityStatus();

    }

}
