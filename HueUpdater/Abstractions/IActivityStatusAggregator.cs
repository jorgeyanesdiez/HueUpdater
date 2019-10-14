namespace HueUpdater.Abstractions
{

    /// <summary>
    /// Contract for services that derive an activity status.
    /// </summary>
    /// <typeparam name="TActivityStatus">The type of the activity status.</typeparam>
    public interface IActivityStatusAggregator<TActivityStatus>
    {

        /// <summary>
        /// Derives an activity status.
        /// </summary>
        /// <returns>The requested value.</returns>
        TActivityStatus GetActivityStatus();

    }

}
