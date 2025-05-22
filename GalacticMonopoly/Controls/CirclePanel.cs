// Controls/CirclePanel.cs
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controls
{
    public class CirclePanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            // Nie propagujemy infinity dalej
            var sizeForChildren = new Size(
                double.IsInfinity(availableSize.Width) ? 0 : availableSize.Width,
                double.IsInfinity(availableSize.Height) ? 0 : availableSize.Height);

            foreach (UIElement child in InternalChildren)
                child.Measure(sizeForChildren);

            // Zwracamy maksymalny rozmiar, jaki chcemy zająć
            return sizeForChildren;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            int count = InternalChildren.Count;
            if (count == 0) return finalSize;

            double angleStep = 360.0 / count;
            double angleOffset = 180; // start from top
            double radiusX = finalSize.Width / 2.5;
            double radiusY = finalSize.Height / 2.5;
            Point center = new(finalSize.Width / 2, finalSize.Height / 2);

            for (int i = 0; i < count; i++)
            {
                var child = InternalChildren[i];
                double angleDeg = angleOffset + i * angleStep;
                double angleRad = angleDeg * Math.PI / 180;

                double x = center.X + radiusX * Math.Cos(angleRad) - child.DesiredSize.Width / 2;
                double y = center.Y + radiusY * Math.Sin(angleRad) - child.DesiredSize.Height / 2;

                var rotate = new RotateTransform(angleDeg + 90,
                    child.DesiredSize.Width / 2,
                    child.DesiredSize.Height / 2);
                child.RenderTransform = rotate;

                child.Arrange(new Rect(new Point(x, y), child.DesiredSize));
            }

            return finalSize;
        }
    }
}
