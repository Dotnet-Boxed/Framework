namespace Boilerplate.Mapping.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Xunit;

    public class AsyncMapperTest
    {
        [Fact]
        public void Map_Null_ThrowsArgumentNullException()
        {
            var mapper = new AsyncMapper();

            Assert.ThrowsAsync<ArgumentNullException>("source", () => mapper.Map(null));
        }

        [Fact]
        public async Task Map_ToNewObject_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.Map(new MapFrom() { Property = 1 });

            Assert.Equal(1, to.Property);
        }

        [Fact]
        public async Task MapArray_Empty_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapArray(
                new MapFrom[0]);

            Assert.IsType<MapTo[]>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapArray_ToNewObject_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapArray(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 }
                });

            Assert.IsType<MapTo[]>(to);
            Assert.Equal(2, to.Length);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapTypedCollection_Empty_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapCollection(
                new MapFrom[0],
                new List<MapTo>());

            Assert.IsType<List<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapTypedCollection_ToNewObject_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapCollection(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 }
                },
                new List<MapTo>());

            Assert.IsType<List<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapCollection_Empty_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapCollection(
                new MapFrom[0]);

            Assert.IsType<Collection<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapCollection_ToNewObject_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapCollection(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 }
                });

            Assert.IsType<Collection<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapList_Empty_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapList(
                new MapFrom[0]);

            Assert.IsType<List<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapList_ToNewObject_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapList(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 }
                });

            Assert.IsType<List<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task MapObservableCollection_Empty_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapObservableCollection(
                new MapFrom[0]);

            Assert.IsType<ObservableCollection<MapTo>>(to);
            Assert.Empty(to);
        }

        [Fact]
        public async Task MapObservableCollection_ToNewObject_Mapped()
        {
            var mapper = new AsyncMapper();

            var to = await mapper.MapObservableCollection(
                new MapFrom[]
                {
                    new MapFrom() { Property = 1 },
                    new MapFrom() { Property = 2 }
                });

            Assert.IsType<ObservableCollection<MapTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }
    }
}
