namespace Boilerplate.Test.ComponentModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Framework;
    using Xunit;

    public class AsyncTranslatorTest
    {
        [Fact]
        public void Translate_Null_ThrowsArgumentNullException()
        {
            var translator = new AsyncTranslator();

            Assert.ThrowsAsync<ArgumentNullException>("source", () => translator.Translate(null));
        }

        [Fact]
        public async Task Translate_ToNewObject_Translated()
        {
            var translator = new AsyncTranslator();

            var to = await translator.Translate(new TranslateFrom() { Property = 1 });

            Assert.Equal(1, to.Property);
        }

        [Fact]
        public async Task TranslateArray_Empty_Translated()
        {
            var translator = new AsyncTranslator();

            var to = await translator.TranslateArray(
                new TranslateFrom[0]);

            Assert.IsType<TranslateTo[]>(to);
            Assert.Equal(0, to.Length);
        }

        [Fact]
        public async Task TranslateArray_ToNewObject_Translated()
        {
            var translator = new AsyncTranslator();

            var to = await translator.TranslateArray(
                new TranslateFrom[]
                {
                    new TranslateFrom() { Property = 1 },
                    new TranslateFrom() { Property = 2 }
                });

            Assert.IsType<TranslateTo[]>(to);
            Assert.Equal(2, to.Length);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task TranslateTypedCollection_Empty_Translated()
        {
            var translator = new AsyncTranslator();

            var to = await translator.TranslateCollection<List<TranslateTo>, TranslateFrom, TranslateTo>(
                new TranslateFrom[0]);

            Assert.IsType<List<TranslateTo>>(to);
            Assert.Equal(0, to.Count);
        }

        [Fact]
        public async Task TranslateTypedCollection_ToNewObject_Translated()
        {
            var translator = new AsyncTranslator();

            var to = await translator.TranslateCollection<List<TranslateTo>, TranslateFrom, TranslateTo>(
                new TranslateFrom[]
                {
                    new TranslateFrom() { Property = 1 },
                    new TranslateFrom() { Property = 2 }
                });

            Assert.IsType<List<TranslateTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task TranslateCollection_Empty_Translated()
        {
            var translator = new AsyncTranslator();

            var to = await translator.TranslateCollection(
                new TranslateFrom[0]);

            Assert.IsType<Collection<TranslateTo>>(to);
            Assert.Equal(0, to.Count);
        }

        [Fact]
        public async Task TranslateCollection_ToNewObject_Translated()
        {
            var translator = new AsyncTranslator();

            var to = await translator.TranslateCollection(
                new TranslateFrom[]
                {
                    new TranslateFrom() { Property = 1 },
                    new TranslateFrom() { Property = 2 }
                });

            Assert.IsType<Collection<TranslateTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }

        [Fact]
        public async Task TranslateList_Empty_Translated()
        {
            var translator = new AsyncTranslator();

            var to = await translator.TranslateList(
                new TranslateFrom[0]);

            Assert.IsType<List<TranslateTo>>(to);
            Assert.Equal(0, to.Count);
        }

        [Fact]
        public async Task TranslateList_ToNewObject_Translated()
        {
            var translator = new AsyncTranslator();

            var to = await translator.TranslateList(
                new TranslateFrom[]
                {
                    new TranslateFrom() { Property = 1 },
                    new TranslateFrom() { Property = 2 }
                });

            Assert.IsType<List<TranslateTo>>(to);
            Assert.Equal(2, to.Count);
            Assert.Equal(1, to[0].Property);
            Assert.Equal(2, to[1].Property);
        }
    }
}
