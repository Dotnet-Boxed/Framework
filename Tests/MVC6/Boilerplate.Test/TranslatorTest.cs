namespace Boilerplate.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Boilerplate;
    using Xunit;

    public class TranslatorTest
    {
        [Fact]
        public void Translate_Null_ThrowsArgumentNullException()
        {
            var translator = new Translator();

            Assert.Throws<ArgumentNullException>("source", () => translator.Translate(null));
        }

        [Fact]
        public void Translate_ToNewObject_Translated()
        {
            var translator = new Translator();

            var to = translator.Translate(new TranslateFrom() { Property = 1 });

            Assert.Equal(1, to.Property);
        }

        [Fact]
        public void TranslateArray_Empty_Translated()
        {
            var translator = new Translator();

            var to = translator.TranslateArray(
                new TranslateFrom[0]);

            Assert.IsType<TranslateTo[]>(to);
            Assert.Equal(0, to.Length);
        }

        [Fact]
        public void TranslateArray_ToNewObject_Translated()
        {
            var translator = new Translator();

            var to = translator.TranslateArray(
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
        public void TranslateTypedCollection_Empty_Translated()
        {
            var translator = new Translator();

            var to = translator.TranslateCollection<List<TranslateTo>, TranslateFrom, TranslateTo>(
                new TranslateFrom[0]);

            Assert.IsType<List<TranslateTo>>(to);
            Assert.Equal(0, to.Count);
        }

        [Fact]
        public void TranslateTypedCollection_ToNewObject_Translated()
        {
            var translator = new Translator();

            var to = translator.TranslateCollection<List<TranslateTo>, TranslateFrom, TranslateTo>(
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
        public void TranslateCollection_Empty_Translated()
        {
            var translator = new Translator();

            var to = translator.TranslateCollection(
                new TranslateFrom[0]);

            Assert.IsType<Collection<TranslateTo>>(to);
            Assert.Equal(0, to.Count);
        }

        [Fact]
        public void TranslateCollection_ToNewObject_Translated()
        {
            var translator = new Translator();

            var to = translator.TranslateCollection(
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
        public void TranslateList_Empty_Translated()
        {
            var translator = new Translator();

            var to = translator.TranslateList(
                new TranslateFrom[0]);

            Assert.IsType<List<TranslateTo>>(to);
            Assert.Equal(0, to.Count);
        }

        [Fact]
        public void TranslateList_ToNewObject_Translated()
        {
            var translator = new Translator();

            var to = translator.TranslateList(
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
