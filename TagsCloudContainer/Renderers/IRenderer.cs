using System.Drawing;

namespace TagsCloudContainer.Renderers;

public interface IRenderer
{
    void AddWord(Word word);
    void SaveImage(string filename);
    Size GetStringSize(string text, int fontSize);
}