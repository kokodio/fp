using System.Drawing;

namespace TagsCloudContainer.Layouters;

public interface ILayouter
{
    Result<Rectangle> PutNextRectangle(Size wordSize);
}