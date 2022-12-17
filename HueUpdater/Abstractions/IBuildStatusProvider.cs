namespace HueUpdater.Abstractions
{

    /// <summary>
    /// Contract for services that provide a build status.
    /// </summary>
    /// <typeparam name="TBuildStatus">The type of the build status.</typeparam>
    public interface IBuildStatusProvider<out TBuildStatus>
    {

        /// <summary>
        /// Provides a build status.
        /// </summary>
        /// <returns>The requested value.</returns>
        TBuildStatus GetBuildStatus();

    }

}
