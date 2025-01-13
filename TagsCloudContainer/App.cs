using TagsCloudContainer.Layouters.Factory;
using TagsCloudContainer.Renderers;
using TagsCloudContainer.WordsPreprocessor;
using TagsCloudContainer.TextProviders.Factory;

namespace TagsCloudContainer;

public class App(
    IRenderer render,
    ILayouterFactory layoutFactory,
    IWordsProviderFactory wordsProviderFactory,
    IWordsPreprocessor wordsFormatter,
    AppConfig appConfig)
{
    public Result<string> Run()
    {
        return appConfig.Validate()
            .Then(_ => wordsProviderFactory.CreateProvider(appConfig.TextFilePath))
            .Then(wordsProvider => wordsProvider.GetWords())
            .Then(wordsFormatter.PreprocessWords)
            .Then(CalculateWordFrequency)
            .Then(RenderImage)
            .OnFail(Console.WriteLine);
    }

    private Result<string> RenderImage(Dictionary<string, int> histogram)
    {
        var layout = layoutFactory.CreateLayouter();
        if (!layout.IsSuccess)
        {
            return Result.Fail<string>(layout.Error);
        }

        var maxCount = histogram.Values.Max();

        foreach (var (text, count) in histogram)
        {
            var fontSize = CalculateFontSize(count, maxCount);
            var stringSize = render.GetStringSize(text, fontSize);
            var position = layout.Value.PutNextRectangle(stringSize);

            if (!position.IsSuccess)
            {
                return Result.Fail<string>(position.Error);
            }
            
            var word = new Word(text, fontSize, position.Value);
            render.AddWord(word);
        }

        var path = Path.Combine(appConfig.OutputPath, appConfig.Filename);
        render.SaveImage(path);

        return Result.Ok($"Картинка успешно сохранена в {appConfig.Filename}");
    }

    private Result<Dictionary<string, int>> CalculateWordFrequency(IEnumerable<string> words)
    {
        Dictionary<string, int> histogram = new();
        var count = 0;

        foreach (var word in words)
        {
            if (!histogram.TryAdd(word, 1))
            {
                histogram[word]++;
            }

            count++;
        }

        return count == 0
            ? Result.Fail<Dictionary<string, int>>("Файл не содержит текста")
            : histogram;
    }

    private int CalculateFontSize(int count, int maxCount) =>
        appConfig.MinSize + (appConfig.MaxSize - appConfig.MinSize) * (count - 1) / maxCount;
}