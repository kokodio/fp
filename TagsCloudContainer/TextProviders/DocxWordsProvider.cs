using DocumentFormat.OpenXml.Packaging;

namespace TagsCloudContainer.TextProviders;

public class DocxWordsProvider(AppConfig appConfig) : IWordsProvider
{
    private readonly char[] separators = [' ', '\t', '\r', '\n'];

    public Result<IEnumerable<string>> GetWords()
    {
        if (!File.Exists(appConfig.TextFilePath))
            return Result.Fail<IEnumerable<string>>($"Файл '{appConfig.TextFilePath}' не найден.");

        return Result.Of(() => ReadDocxFile(appConfig.TextFilePath))
            .Then(text => text
                .Split(separators, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.IsNullOrWhiteSpace(x)));
    }

    private static string ReadDocxFile(string filePath)
    {
        using var wordDoc = WordprocessingDocument.Open(filePath, false);
        var body = wordDoc.MainDocumentPart.Document.Body;
        return body.InnerText;
    }
}