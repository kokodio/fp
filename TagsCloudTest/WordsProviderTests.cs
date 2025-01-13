using FluentAssertions;
using TagsCloudContainer;
using TagsCloudContainer.TextProviders;
using TagsCloudContainer.TextProviders.Factory;

namespace TagsCloudTest;

public class WordsProviderTests
{
    [Test]
    public void GetWords_ReturnSeparatedWordsFromTxt()
    {
        var appConfig = new AppConfig
        {
            TextFilePath = "./ProviderTest.txt"
        };
        var factory = new WordsProviderFactory(appConfig);
        
        factory.CreateProvider(appConfig.TextFilePath)
            .Then(provider => provider.GetWords())
            .Then(words => words.Should()
            .BeEquivalentTo(["a", "b", "c", "a", "b", "c"]));
    }
    
    [Test]
    public void GetWords_ReturnSeparatedWordsFromDocx()
    {
        var appConfig = new AppConfig
        {
            TextFilePath = "./docx.docx"
        };
        var factory = new WordsProviderFactory(appConfig);
        
        factory.CreateProvider(appConfig.TextFilePath)
            .Then(provider => provider.GetWords())
            .Then(words => words.Should()
            .BeEquivalentTo(["очень", "маленький", "тестовый", "файл", "для", "docx"]));
        
    }

    [Test]
    public void CreateProvider_ThrowExceptionIfFileDoesNotExist()
    {
        var appConfig = new AppConfig
        {
            TextFilePath = "./DoNotExist.txt"
        };
        var provider = new WordsProviderFactory(appConfig);
        
        var result = provider.CreateProvider(appConfig.TextFilePath);
        
        result.Should().BeEquivalentTo(Result.Fail<IWordsProvider>($"Файл '{appConfig.TextFilePath}' не найден."));
    }
    
    [Test]
    public void CreateProvider_ThrowExceptionIfNonTxtFile()
    {
        var appConfig = new AppConfig
        {
            TextFilePath = "./Autofac.dll"
        };
        var provider = new WordsProviderFactory(appConfig);
        
        var result = provider.CreateProvider(appConfig.TextFilePath);
        
        result.Should().BeEquivalentTo(Result.Fail<IWordsProvider>($"Неподдерживаемый формат файла: .dll"));
    }
    
    [Test]
    public void CreateProvider_ThrowExceptionIfPathDoesNotExist()
    {
        var appConfig = new AppConfig
        {
            TextFilePath = ""
        };
        var provider = new WordsProviderFactory(appConfig);
        
        var result = provider.CreateProvider(appConfig.TextFilePath);
        
        result.Should().BeEquivalentTo(Result.Fail<IWordsProvider>($"Отсутствует путь к файлу"));
    }
}