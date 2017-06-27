namespace Boilerplate.Translation.Test
{
    using System.Threading.Tasks;

    public class AsyncTranslator : IAsyncTranslator<TranslateFrom, TranslateTo>
    {
        public Task Translate(TranslateFrom from, TranslateTo to)
        {
            to.Property = from.Property;
            return Task.FromResult<object>(null);
        }
    }
}
