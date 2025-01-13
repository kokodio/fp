using System.Drawing;
using FluentAssertions;
using TagsCloudContainer;
using TagsCloudContainer.Layouters;
using TagsCloudContainer.Layouters.Factory;
using TagsCloudContainer.Renderers;
using TagsCloudContainer.TextProviders.Factory;
using TagsCloudContainer.WordsPreprocessor;

namespace TagsCloudTest;

public class AppTests
{
    private App app;
    private AppConfig config;

    [SetUp]
    public void Setup()
    {
        config = new AppConfig
        {
            TextFilePath = "./ProviderTest.txt",
        };

        var renderer = new SimpleRenderer(config);
        var layouter = new LayouterFactory(config);
        var provider = new WordsProviderFactory(config);
        var preprocessor = new WordsPreprocessor(config);
        app = new App(renderer, layouter, provider, preprocessor, config);
    }

    [Test]
    public void Run_GenerateWordCloudFromTxtFile()
    {
        app.Run();

        var fileExists = File.Exists("render.png");
        fileExists.Should().BeTrue("The program should generate a PNG file.");
    }

    [Test]
    public void Run_GenerateBlueBackground()
    {
        config.BackgroundColor = "Blue";
        app.Run();

        using var bitmap = new Bitmap("render.png");

        var backgroundColor = bitmap.GetPixel(0, 0);
        backgroundColor.ToArgb().Should().Be(Color.Blue.ToArgb());
    }

    [Test]
    public void Run_GenerateBlueText()
    {
        config.TextColor = "Blue";
        app.Run();

        using var bitmap = new Bitmap("render.png");

        var backgroundColor = bitmap.GetPixel(0, 0);
        backgroundColor.ToArgb().Should().NotBe(Color.Blue.ToArgb());
        var containsBlue = false;


        for (var i = 0; i < bitmap.Width; i++)
        for (var j = 0; j < bitmap.Height; j++)
        {
            if (bitmap.GetPixel(i, j).ToArgb() == Color.Blue.ToArgb())
            {
                containsBlue = true;
                goto exit;
            }
        }

        exit:
        containsBlue.Should().BeTrue();
    }
}