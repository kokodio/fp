using System.Text.RegularExpressions;

namespace TagsCloudContainer.WordsPreprocessor;

public partial class WordsPreprocessor(AppConfig appConfig) : IWordsPreprocessor
{
    public IEnumerable<string> PreprocessWords(IEnumerable<string> words)
    {
        return words
            .Select(x => x.ToLower())
            .Select(x => MyRegex().Replace(x, "").Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Where(x => !appConfig.ExcludedWords.Contains(x));
    }

    [GeneratedRegex(@"[^\p{L}]")]
    private static partial Regex MyRegex();
}