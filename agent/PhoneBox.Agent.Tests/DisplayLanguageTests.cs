using System.Text.Json;
using PhoneBox.Agent;
using Xunit;

public sealed class DisplayLanguageTests
{
    [Theory]
    [InlineData("ar", "ar")]
    [InlineData(" AR ", "ar")]
    [InlineData("de", "de")]
    [InlineData("en", "en")]
    [InlineData("unknown", "auto")]
    public void LanguageIsNormalizedAndPersistsAfterReload(string submitted, string expected)
    {
        var directory = Directory.CreateTempSubdirectory("pb-display-language-");
        var path = Path.Combine(directory.FullName, "config.json");
        try
        {
            File.WriteAllText(path, "{}");
            var store = new ConfigStore(path);
            var json = JsonSerializer.Serialize(new { display = new { language = submitted } });
            Assert.Null(store.Save(json));
            Assert.Equal(expected, new ConfigStore(path).Get().Display.Language);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }
}
