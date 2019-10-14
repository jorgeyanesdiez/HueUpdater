namespace HueUpdater.Abstractions
{

    /// <summary>
    /// Contract for services that derive a build status.
    /// </summary>
    /// <typeparam name="TBuildStatus">The type of the build status.</typeparam>
    public interface IBuildStatusAggregator<TBuildStatus>
    {

        /// <summary>
        /// Derives a build status.
        /// </summary>
        /// <returns>The requested value.</returns>
        TBuildStatus GetBuildStatus();

    }

}
