using TagsCloudContainer;
using TagsCloudContainer.Layouters.Factory;
using TagsCloudContainer.Renderers;
using TagsCloudContainer.TextProviders.Factory;
using TagsCloudContainer.WordsPreprocessor;

var builder = WebApplication.CreateBuilder(args);

var appConfig = new AppConfig
{
    TextFilePath = "",
    OutputPath = Path.Combine(builder.Environment.WebRootPath, "images")
};


builder.Services.AddControllersWithViews();

builder.Services.AddScoped<App>();
builder.Services.AddScoped<IRenderer, SimpleRenderer>();

builder.Services.AddSingleton(appConfig);
builder.Services.AddSingleton<ILayouterFactory, LayouterFactory>();
builder.Services.AddSingleton<IWordsProviderFactory, WordsProviderFactory>();
builder.Services.AddSingleton<IWordsPreprocessor, WordsPreprocessor>();

var app = builder.Build();

if (!Directory.Exists(appConfig.OutputPath))
{
    Directory.CreateDirectory(appConfig.OutputPath);
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TagsCloud}/{action=Index}");

app.Run();