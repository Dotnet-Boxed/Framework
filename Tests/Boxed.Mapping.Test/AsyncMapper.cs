namespace Boxed.Mapping.Test
{
    using System;
    using System.Threading.Tasks;

    public class AsyncMapper : IAsyncMapper<MapFrom, MapTo>
    {
        public Task MapAsync(MapFrom from, MapTo to)
        {
            if (from is null)
            {
                throw new ArgumentNullException(nameof(from));
            }

            if (to is null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            to.Property = from.Property;
            return Task.FromResult<object>(null);
        }
    }
}
