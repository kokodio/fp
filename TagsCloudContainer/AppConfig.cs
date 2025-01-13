using System.Collections.Frozen;
using TagsCloudContainer.Layouters;

namespace TagsCloudContainer;

public class AppConfig
{
    public LayoutType LayoutType { get; set; } = LayoutType.Circular;
    public int Width { get; set; } = 3000;
    public int Height { get; set; } = 3000;
    public int MaxSize { get; set; } = 100;
    public int MinSize { get; set; } = 10;
    public double AngleStep { get; set; } = 0.01;
    public double RadiusStep { get; set; } = 0.01;
    public string FontFamily { get; set; } = "Arial";
    public string TextFilePath { get; set; } = "./text.txt";
    public string OutputPath { get; set; } = "./";
    public string Filename { get; set; } = "render.png";
    public string BackgroundColor { get; set; } = "#000000";
    public string TextColor { get; set; } = "#FFFFFF";

    public FrozenSet<string> ExcludedWords { get; set; } = new[]
    {
        "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф",
        "х", "ц", "ч", "ш", "щ", "ы", "э", "ю", "я",
        "на", "об", "от", "до", "по", "из", "за", "над", "под", "при", "про", "для", "без", "мимо", "через",
        "я", "ты", "мы", "вы", "он", "она", "оно", "они", "мой", "твой", "наш", "ваш", "его", "её", "их", "сам", "кто",
        "что", "чей", "какой",
        "и", "не", "а", "бы", "так", "ему", "же", "но", "им", "то", "этот", "этой", "чьей", "это", "кого",
    }.ToFrozenSet();
    
    public Result<None> Validate()
    {
        
        if (MaxSize <= 0 || MinSize <= 0)
        {
            return Result.Fail<None>("The size cannot be less than 1");
        }
        if (MaxSize < MinSize)
        {
            return Result.Fail<None>("MaxSize cannot be smaller than MinSize");
        }
            
        
        return Result.Ok();
    }
}
