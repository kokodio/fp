namespace TagsCloudContainer.TextProviders.Factory;

public class WordsProviderFactory(AppConfig appConfig) : IWordsProviderFactory
{
    public Result<IWordsProvider> CreateProvider(string filePath)
    {
        if (string.IsNullOrEmpty(appConfig?.TextFilePath))
        {
            return Result.Fail<IWordsProvider>("Отсутствует путь к файлу");
        }

        if (!File.Exists(appConfig.TextFilePath))
        {
            return Result.Fail<IWordsProvider>($"Файл '{appConfig.TextFilePath}' не найден.");
        }
        
        var fileExtension = Path.GetExtension(filePath)?.ToLowerInvariant();
        
        return fileExtension switch
        {
            ".docx" => new DocxWordsProvider(appConfig),
            ".txt" => new TxtWordsProvider(appConfig),
            _ => Result.Fail<IWordsProvider>($"Неподдерживаемый формат файла: {fileExtension}")
        };
    }
}