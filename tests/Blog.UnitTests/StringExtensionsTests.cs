using Blog.Domain.Extensions;
using FluentAssertions;
using Xunit;

namespace Blog.UnitTests;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("", "", 50)]
    [InlineData(" ", " ", 50)]
    [InlineData("Testo di esempio minore di 50 caratteri", "Testo di esempio minore di 50 caratteri", 50)]
    [InlineData("Testo con tag rotto <strong> minore di 50", "Testo con tag rotto minore di 50", 50)]
    [InlineData("Testo con <b>tag</b> corretto < 50", "Testo con tag corretto < 50", 50)]
    [InlineData(@"Esempio di testo con <strong> tag maggiore di <a href=""#""> 50 caratteri</a></strong>.", "Esempio di testo con tag maggiore di 50&hellip;", 50)]
    [InlineData(@"Lorem <strong>ipsum dolor sit amet consectetur</strong> adipisicing elit. Nulla, optio placeat laudantium aliquam esse harum? Eius porro cupiditate <em></em>similique cum numquam corrupti iure <i>quibusdam magnam</i> voluptatum necessitatibus, neque cumque pariatur!", "Lorem ipsum dolor sit amet consectetur&hellip;", 50)]
    [InlineData(@"Testo con <a href="""">lunghezza superiore a 50 caratteri con punto</a>. Tutto quello che viene dopo � ignorato.", "Testo con lunghezza superiore a 50 caratteri con&hellip;", 50)]
    [InlineData(@"Testo con <strong>punto</strong> alla fine di 50 caratteri. 123456789", "Testo con punto alla fine di 50 caratteri&hellip;", 50)]
    [InlineData(@"La Biennale di Venezia, nella ricorrenza  dsfsf�! i test, anni dalla sua fondazione, presenta la mostra Le muse inquiete. La Biennale di fronte alla storia,", "La Biennale di Venezia, nella ricorrenza dsfsf�&hellip;", 50)]
    public void TruncateHtmlTest(string input, string output, int maxCharacters)
    {
        var result = input.TrimAndTruncateHtml(maxCharacters);
        result.Should().Be(output);
    }
}