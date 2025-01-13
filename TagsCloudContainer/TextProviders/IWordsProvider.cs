namespace TagsCloudContainer.TextProviders;

public interface IWordsProvider
{
    Result<IEnumerable<string>> GetWords();
}