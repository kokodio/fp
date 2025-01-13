namespace TagsCloudContainer.WordsPreprocessor;

public interface IWordsPreprocessor
{
    IEnumerable<string> PreprocessWords(IEnumerable<string> words);
}