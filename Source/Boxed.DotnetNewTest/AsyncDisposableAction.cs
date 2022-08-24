namespace Boxed.DotnetNewTest;

using System;
using System.Threading.Tasks;

/// <summary>
/// Executes an asynchronous operation when disposed.
/// </summary>
/// <seealso cref="IAsyncDisposable" />
internal sealed class AsyncDisposableAction : IAsyncDisposable
{
    private readonly Func<ValueTask> action;

    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncDisposableAction"/> class.
    /// </summary>
    /// <param name="action">The action.</param>
    /// <exception cref="ArgumentNullException"><paramref name="action"/> is <c>null</c>.</exception>
    public AsyncDisposableAction(Func<ValueTask> action)
    {
        ArgumentNullException.ThrowIfNull(action);

        this.action = action;
    }

    /// <summary>
    /// Executes the asynchronous operation.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    public ValueTask DisposeAsync() => this.action();
}
