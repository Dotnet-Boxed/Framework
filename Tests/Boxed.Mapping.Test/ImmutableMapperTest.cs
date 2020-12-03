namespace Boxed.Mapping.Test
{
    using System;
    using System.Collections.Generic;
#if NET5_0
    using System.Collections.Immutable;
#endif
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ImmutableMapperTest
    {
        [Fact]
        public void Map_Null_ThrowsArgumentNullException()
        {
            var mapper = new ImmutableMapper();

            Assert.Throws<ArgumentNullException>("from", () => mapper.Map(null));
        }

        [Fact]
        public void Map_ToNewObject_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.Map(new MapFrom() { Property = 1 });

            Assert.Equal(1, to.Property);
        }

        [Fact]
        public void MapArray_Empty_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapArray(Array.Empty<MapFrom>());

            Assert.IsType<MapTo[]>(to);
            Assert.Empty(to);
        }

        [Fact]
        public void MapArray_ToNewObject_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapArray(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 },
                });

            Assert.IsType<MapTo[]>(to);
            Assert.Equal(2, to.Length);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public void MapTypedCollection_Empty_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapCollection(
                Array.Empty<MapFrom>(),
                new List<MapTo>());

            Assert.IsType<List<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public void MapTypedCollection_ToNewObject_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapCollection(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 },
                },
                new List<MapTo>());

            Assert.IsType<List<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public void MapCollection_Empty_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapCollection(Array.Empty<MapFrom>());

            Assert.IsType<Collection<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public void MapCollection_ToNewObject_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapCollection(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 },
                });

            Assert.IsType<Collection<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public void MapHashSet_Empty_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapHashSet(Array.Empty<MapFrom>());

            Assert.IsType<HashSet<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public void MapHashSet_ToNewObject_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapHashSet(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 },
                });

            Assert.IsType<HashSet<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(new int[] { 1, 2 }, to.Select(x => x.Property));
        }

#if NET5_0
        [Fact]
        public void MapImmutableArray_Empty_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapImmutableArray(Array.Empty<MapFrom>());

            Assert.IsType<ImmutableArray<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public void MapImmutableArray_ToNewObject_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapImmutableArray(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 },
                });

            Assert.IsType<ImmutableArray<MapTo>>(to);
            Assert.Equal(2, to.Length);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }
#endif

        [Fact]
        public void MapList_Empty_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapList(Array.Empty<MapFrom>());

            Assert.IsType<List<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public void MapList_ToNewObject_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapList(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 },
                });

            Assert.IsType<List<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public void MapObservableCollection_Empty_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapObservableCollection(Array.Empty<MapFrom>());

            Assert.IsType<ObservableCollection<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public void MapObservableCollection_ToNewObject_Mapped()
        {
            var mapper = new ImmutableMapper();

            var to = mapper.MapObservableCollection(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 },
                });

            Assert.IsType<ObservableCollection<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapAsyncEnumerable_ToNewObject_MappedAsync()
        {
            var mapper = new ImmutableMapper();
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
