using System.Text;

namespace BloomreachTests.Utils;

public class TestUtils
{
    public static string ReadFile(string fileName)
    {
        return File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, $@"Jsons/{fileName}.json"));
    }
}