using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudContainer.Renderers;

public class SimpleRenderer(AppConfig appConfig) : IRenderer
{
    private readonly List<Word> words = [];

    public void AddWord(Word word) => words.Add(word);

    public void SaveImage(string filename)
    {
        using var bitmap = new Bitmap(appConfig.Width, appConfig.Height);
        using var graphics = Graphics.FromImage(bitmap);

        FillBackground(graphics, appConfig.Width, appConfig.Height);
        DrawWords(graphics);

        var path = Path.Combine(appConfig.OutputPath, appConfig.Filename);
        bitmap.Save(path, ImageFormat.Png);
    }

    public Size GetStringSize(string text, int fontSize)
    {
        using var font = new Font(appConfig.FontFamily, fontSize);
        using var graphics = Graphics.FromHwnd(IntPtr.Zero);
        var size = graphics.MeasureString(text, font);

        return size.ToSize();
    }

    private void FillBackground(Graphics graphics, int width, int height)
    {
        var brushColor = ColorTranslator.FromHtml(appConfig.BackgroundColor);
        using var brush = new SolidBrush(brushColor);

        graphics.FillRectangle(brush, 0, 0, width, height);
    }

    private void DrawWords(Graphics graphics)
    {
        var brushColor = ColorTranslator.FromHtml(appConfig.TextColor);
        using var brush = new SolidBrush(brushColor);

        foreach (var word in words)
        {
            using var font = new Font(appConfig.FontFamily, word.FontSize);
            var x = word.Position.X + appConfig.Width / 2;
            var y = word.Position.Y + appConfig.Height / 2;

            graphics.DrawString(word.Text, font, brush, x, y);
        }
    }
}