namespace Boxed.Mapping.Test
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class TestAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly IEnumerable<T> items;

        public TestAsyncEnumerable(IEnumerable<T> items) => this.items = items;

        public async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            foreach (var item in this.items)
            {
                await Task.Delay(0, cancellationToken).ConfigureAwait(false);
                yield return item;
            }
        }
    }
}
