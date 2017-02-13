namespace Boilerplate.Test
{
    using System.Threading.Tasks;
    using Boilerplate;

    public class AsyncTranslator : IAsyncTranslator<TranslateFrom, TranslateTo>
    {
        public Task Translate(TranslateFrom from, TranslateTo to)
        {
            to.Property = from.Property;
            return Task.FromResult<object>(null);
        }
    }
}
