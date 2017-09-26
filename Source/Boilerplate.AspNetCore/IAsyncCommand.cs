namespace Boilerplate.AspNetCore
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Executes a single command and returns a result.
    /// </summary>
    public interface IAsyncCommand
    {
        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the command.</returns>
        Task<IActionResult> ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
