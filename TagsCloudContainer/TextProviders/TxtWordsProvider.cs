using System.Text;

namespace TagsCloudContainer.TextProviders;

public class TxtWordsProvider(AppConfig appConfig) : IWordsProvider
{
    private readonly char[] separators = [' ', '\t', '\r', '\n'];

    public Result<IEnumerable<string>> GetWords()
    {
        if (!File.Exists(appConfig.TextFilePath))
            return Result.Fail<IEnumerable<string>>($"Файл '{appConfig.TextFilePath}' не найден.");

        return
            Result.Ok(
                File.ReadAllText(appConfig.TextFilePath, Encoding.Default)
                    .Split(separators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
            );
    }
}