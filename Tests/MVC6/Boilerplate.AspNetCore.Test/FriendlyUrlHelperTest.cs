namespace Boilerplate.AspNetCore.Test
{
    using Xunit;

    public class FriendlyUrlHelperTest
    {
        [Theory]

        // Null or empty.
        [InlineData(null, "")]
        [InlineData("", "")]

        // Alphanumeric characters.
        [InlineData("abcdefghijkl`mnopqrstuvwxyz", "abcdefghijklmnopqrstuvwxyz")]
        [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz")]
        [InlineData("1234567890", "1234567890")]

        // Special characters with no preceding valid character.
        [InlineData(" ", "")]
        [InlineData(",", "")]
        [InlineData(" ", "")]
        [InlineData(".", "")]
        [InlineData("/", "")]
        [InlineData("\\", "")]
        [InlineData("-", "")]
        [InlineData("_", "")]
        [InlineData("=", "")]

        // Special characters with preceding valid character followed by space.
        [InlineData("a  b", "a-b")]
        [InlineData("a ,b", "a-b")]
        [InlineData("a  b", "a-b")]
        [InlineData("a .b", "a-b")]
        [InlineData("a /b", "a-b")]
        [InlineData("a \\b", "a-b")]
        [InlineData("a -b", "a-b")]
        [InlineData("a _b", "a-b")]
        [InlineData("a =b", "a-b")]

        // Special characters with preceding valid character.
        [InlineData("a b", "a-b")]
        [InlineData("a,b", "a-b")]
        [InlineData("a b", "a-b")]
        [InlineData("a.b", "a-b")]
        [InlineData("a/b", "a-b")]
        [InlineData("a\\b", "a-b")]
        [InlineData("a-b", "a-b")]
        [InlineData("a_b", "a-b")]
        [InlineData("a=b", "a-b")]

        // Invalid characters
        [InlineData("a`¬!\"£$%^&*()+{}[]:@;'<>?#~|b", "ab")]

        // Real world usage
        [InlineData("hello", "hello")]
        [InlineData("HELLO", "hello")]
        [InlineData("Hello World", "hello-world")]
        [InlineData(" Hello World ", "hello-world")]

        // Characters mappable to ASCII
        [InlineData("àåáâäãåąā", "aaaaaaaaa")]
        [InlineData("èéêěëę", "eeeeee")]
        [InlineData("ìíîïı", "iiiii")]
        [InlineData("òóôõöøőð", "oooooooo")]
        [InlineData("ùúûüŭů", "uuuuuu")]
        [InlineData("çćčĉ", "cccc")]
        [InlineData("żźž", "zzz")]
        [InlineData("śşšŝ", "ssss")]
        [InlineData("ñń", "nn")]
        [InlineData("ýÿ", "yy")]
        [InlineData("ğĝ", "gg")]
        [InlineData("ŕř", "rr")]
        [InlineData("ĺľł", "lll")]
        [InlineData("úů", "uu")]
        [InlineData("đď", "dd")]
        [InlineData("ť", "t")]
        [InlineData("ž", "z")]
        [InlineData("ß", "ss")]
        [InlineData("Þ", "th")]
        [InlineData("ĥ", "h")]
        [InlineData("ĵ", "j")]
        public void GetFriendlyTitle_RemapToAscii_ReturnsExpectedFriendlyTitle(string title, string expectedFriendlyTitle)
        {
            var actualFriendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(title, true, 100);

            Assert.Equal(expectedFriendlyTitle, actualFriendlyTitle);
        }

        [Theory]
        [InlineData("àåáâäãåąā", "àåáâäãåąā")]
        [InlineData("èéêěëę", "èéêěëę")]
        [InlineData("ìíîïı", "ìíîïı")]
        [InlineData("òóôõöøőð", "òóôõöøőð")]
        [InlineData("ùúûüŭů", "ùúûüŭů")]
        [InlineData("çćčĉ", "çćčĉ")]
        [InlineData("żźž", "żźž")]
        [InlineData("śşšŝ", "śşšŝ")]
        [InlineData("ñń", "ñń")]
        [InlineData("ýÿ", "ýÿ")]
        [InlineData("ğĝ", "ğĝ")]
        [InlineData("ŕř", "ŕř")]
        [InlineData("ĺľł", "ĺľł")]
        [InlineData("úů", "úů")]
        [InlineData("đď", "đď")]
        [InlineData("ť", "ť")]
        [InlineData("ž", "ž")]
        [InlineData("ß", "ß")]
        [InlineData("Þ", "Þ")]
        [InlineData("ĥ", "ĥ")]
        [InlineData("ĵ", "ĵ")]
        public void GetFriendlyTitle_NoRemapToAscii_ReturnsExpectedFriendlyTitle(string title, string expectedFriendlyTitle)
        {
            var actualFriendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(title, false, 30);

            Assert.Equal(expectedFriendlyTitle, actualFriendlyTitle);
        }

        [Theory]
        [InlineData("abcde", "abcde")]
        [InlineData("abcdef", "abcde")]
        [InlineData("abcß", "abcss")]
        [InlineData("abcdß", "abcds")]
        [InlineData("abcdeß", "abcde")]
        public void GetFriendlyTitle_RemapToAsciiMax5Characters_ReturnsExpectedFriendlyTitle(string title, string expectedFriendlyTitle)
        {
            var actualFriendlyTitle = FriendlyUrlHelper.GetFriendlyTitle(title, true, 5);

            Assert.Equal(expectedFriendlyTitle, actualFriendlyTitle);
        }
    }
}
