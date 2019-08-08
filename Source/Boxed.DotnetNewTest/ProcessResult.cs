namespace Boxed.DotnetNewTest
{
    /// <summary>
    /// The result from starting a process.
    /// </summary>
    public enum ProcessResult
    {
        /// <summary>
        /// The process exited successfully.
        /// </summary>
        Succeeded,

        /// <summary>
        /// The process failed to start or returned a non-zero exit code.
        /// </summary>
        Failed,

        /// <summary>
        /// The process timed out and took too long to shut down.
        /// </summary>
        TimedOut,
    }
}
