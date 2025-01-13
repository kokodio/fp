namespace TagsCloudContainer.TextProviders.Factory;

public interface IWordsProviderFactory
{
    Result<IWordsProvider> CreateProvider(string filePath);
}