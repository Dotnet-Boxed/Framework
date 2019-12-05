namespace Boxed.DotnetNewTest
{
    using System;
    using System.Threading.Tasks;

    internal class AsyncDisposableAction : IAsyncDisposable
    {
        private readonly Func<ValueTask> action;

        public AsyncDisposableAction(Func<ValueTask> action) =>
            this.action = action ?? throw new ArgumentNullException(nameof(action));

        public ValueTask DisposeAsync() => this.action();
    }
}
