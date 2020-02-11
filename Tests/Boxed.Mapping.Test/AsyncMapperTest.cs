namespace Boxed.Mapping.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class AsyncMapperTest
    {
        [Fact]
        public Task MapAsync_Null_ThrowsArgumentNullExceptionAsync()
        {
            var mapper = new AsyncMapper();

            return Assert.ThrowsAsync<ArgumentNullException>("source", () => mapper.MapAsync(null));
        }

        [Fact]
        public async Task MapAsync_ToNewObject_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapAsync(new MapFrom() { Property = 1 }).ConfigureAwait(false);

            Assert.Equal(1, to.Property);
        }

        [Fact]
        public async Task MapArrayAsync_Empty_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapArrayAsync(Array.Empty<MapFrom>()).ConfigureAwait(false);

            Assert.IsType<MapTo[]>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapArrayAsync_ToNewObject_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper
                .MapArrayAsync(
                    new MapFrom[]
                    {
                        new MapFrom() { Property = 1 },
                        new MapFrom() { Property = 2 },
                    })
                .ConfigureAwait(false);

            Assert.IsType<MapTo[]>(to);
            Assert.Equal(2, to.Length);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapTypedCollectionAsync_Empty_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapCollectionAsync(Array.Empty<MapFrom>(), new List<MapTo>()).ConfigureAwait(false);

            Assert.IsType<List<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapTypedCollectionAsync_ToNewObject_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper
                .MapCollectionAsync(
                    new MapFrom[]
                    {
                        new MapFrom() { Property = 1 },
                        new MapFrom() { Property = 2 },
                    },
                    new List<MapTo>())
                .ConfigureAwait(false);

            Assert.IsType<List<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapCollectionAsync_Empty_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapCollectionAsync(Array.Empty<MapFrom>()).ConfigureAwait(false);

            Assert.IsType<Collection<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapCollectionAsync_ToNewObject_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper
                .MapCollectionAsync(
                    new MapFrom[]
                    {
                        new MapFrom() { Property = 1 },
                        new MapFrom() { Property = 2 },
                    })
                .ConfigureAwait(false);

            Assert.IsType<Collection<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapListAsync_Empty_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapListAsync(Array.Empty<MapFrom>()).ConfigureAwait(false);

            Assert.IsType<List<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapListAsync_ToNewObject_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper
                .MapListAsync(
                    new MapFrom[]
                    {
                        new MapFrom() { Property = 1 },
                        new MapFrom() { Property = 2 },
                    })
                .ConfigureAwait(false);

            Assert.IsType<List<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapObservableCollectionAsync_Empty_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper
                .MapObservableCollectionAsync(Array.Empty<MapFrom>())
                .ConfigureAwait(false);

            Assert.IsType<ObservableCollection<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapObservableCollectionAsync_ToNewObject_MappedAsync()
        {
            var mapper = new AsyncMapper();

            var to = await mapper
                .MapObservableCollectionAsync(
                    new MapFrom[]
                    {
                        new MapFrom() { Property = 1 },
                        new MapFrom() { Property = 2 },
                    })
                .ConfigureAwait(false);

            Assert.IsType<ObservableCollection<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapAsyncEnumerable_ToNewObject_MappedAsync()
        {
            var mapper = new AsyncMapper();
            var source = new TestAsyncEnumerable<MapFrom>(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 },
                });

            var to = mapper.MapEnumerableAsync(source);

            var list = await to.ToListAsync().ConfigureAwait(false);
            Assert.IsType<List<MapTo>>(list);
            Assert.Equal(2, list.Count);
            Assert.Equal(1, list[0].Property);
            Assert.Equal(2, list[1].Property);
        }
    }
}
