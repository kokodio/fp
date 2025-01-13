using System.Collections.Frozen;
using FluentAssertions;
using TagsCloudContainer;
using TagsCloudContainer.WordsPreprocessor;

namespace TagsCloudTest;

public class WordsPreprocessorTests
{
    private WordsPreprocessor wordsPreprocessor;

    [SetUp]
    public void Setup()
    {
        var appConfig = new AppConfig();

        wordsPreprocessor = new WordsPreprocessor(appConfig);
    }
    
    [Test]
    public void PreprocessWords_ReturnsSame()
    {
        string[] input = ["просто", "обычные", "слова"];
        
        wordsPreprocessor.PreprocessWords(input)
            .Should()
            .BeEquivalentTo(input);
    }
    
    [Test]
    public void PreprocessWords_ExcludePronouns()
    {
        wordsPreprocessor.PreprocessWords(["он", "она", "они"])
            .Should()
            .BeEmpty();
    }

    [Test]
    public void PreprocessWords_ReturnsNothing_WhenInputIsEmpty()
    {
        wordsPreprocessor.PreprocessWords([])
            .Should()
            .BeEmpty();
    }
    
    [Test]
    public void PreprocessWords_ReturnsNothing_WhenInputIsPunctuation()
    {
        wordsPreprocessor.PreprocessWords(["!", "...", ",./"])
            .Should()
            .BeEmpty();
    }
    
    [Test]
    public void PreprocessWords_ReturnsOnlyAlphabet()
    {
        string[] expected = ["привет", "очень"];
        
        wordsPreprocessor.PreprocessWords(["Привет!", "...", "Очень?"])
            .Should()
            .BeEquivalentTo(expected);
    }

    [Test]
    public void PreprocessWords_ReturnLowerCase()
    {
        string[] input = ["ПрИвет", "оченЬ", "КруЖкА"];
        
        wordsPreprocessor.PreprocessWords(input)
            .Should()
            .BeEquivalentTo(input
                .Select(x => x.ToLower()));
    }
    
    [Test]
    public void PreprocessWords_ExcludeWordsFromConfig()
    {
        var appConfig = new AppConfig
        {
            ExcludedWords = new []{"необычный", "обычный"}.ToFrozenSet()
        };

        wordsPreprocessor = new WordsPreprocessor(appConfig);
        
        string[] input = ["необычный", "обычный", "я"];

        wordsPreprocessor.PreprocessWords(input)
            .Should()
            .BeEquivalentTo(["я"]);
    }
}