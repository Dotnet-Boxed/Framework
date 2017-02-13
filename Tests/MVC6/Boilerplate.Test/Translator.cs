namespace Boilerplate.Test
{
    using Boilerplate;

    public class Translator : ITranslator<TranslateFrom, TranslateTo>
    {
        public void Translate(TranslateFrom from, TranslateTo to)
        {
            to.Property = from.Property;
        }
    }
}
