using System.Drawing;

namespace TagsCloudContainer.Layouters;

public class CircularCloudLayouter(AppConfig appConfig, Point center = new()) : ILayouter
{
    private double angle;
    private double radius;
    private readonly List<Rectangle> rectangles = [];

    public Result<Rectangle> PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
        {
            return Result.Fail<Rectangle>($"{rectangleSize} меньше 0");
        }

        var rectangle = new Rectangle(FindNextLocation(rectangleSize), rectangleSize);

        rectangles.Add(rectangle);

        return rectangle;
    }

    private Point FindNextLocation(Size rectangleSize)
    {
        var location = center;
        Rectangle guessRectangle;

        if (rectangles.Count == 0)
        {
            return location;
        }

        do
        {
            location = GetPointOnSpiral();
            guessRectangle = new Rectangle(location, rectangleSize);
            radius += appConfig.RadiusStep;
            angle += appConfig.AngleStep;
        } while (IsIntersecting(guessRectangle));

        return location;
    }

    private bool IsIntersecting(Rectangle rectangle)
    {
        var isIntersecting = false;
        
        foreach (var outerRectangle in rectangles)
        {
            if (rectangle.IntersectsWith(outerRectangle))
            {
                isIntersecting = true;
                break;
            }
        }
        
        return isIntersecting;
    }

private Point GetPointOnSpiral()
    {
        var (sin, cos) = Math.SinCos(angle);
        var x = (int)(radius * cos) + center.X;
        var y = (int)(radius * sin) + center.Y;
        return new Point(x, y);
    }
}