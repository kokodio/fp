using System.Drawing;

namespace TagsCloudContainer.Layouters.Factory;

public interface ILayouterFactory
{
    public Result<ILayouter> CreateLayouter(Point center = new());
}