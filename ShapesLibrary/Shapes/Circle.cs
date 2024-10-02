using ShapesLibrary.Interfaces;

namespace ShapesLibrary.Shapes
{
    public class Circle : IShape
    {
        public double  Radius { get; set; }
        public Circle(double radius) 
        {
            Radius = radius;
        }

        public double CalculateSquare()
        {
            return Math.PI * Radius * Radius;
        }
    }
}
