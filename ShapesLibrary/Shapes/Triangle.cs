using ShapesLibrary.Interfaces;

namespace ShapesLibrary.Shapes
{
    public class Triangle : IShape
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public Triangle(double a, double b, double c)
        {
            if (!IsValid(a, b, c))
                throw new ArgumentException("Не существует треугольника с заданными сторонами");

            A = a;
            B = b;
            C = c;
        }

        public bool IsValid(double a, double b, double c)
        {
            return a + b > c && b + c > a && a + c > b;
        }
        public bool IsRightAngled()
        {
            var orderedSides = new[] {A, B, C}.OrderBy(s => s).ToArray();
            return Math.Abs(orderedSides[0] * orderedSides[0] + orderedSides[1] * orderedSides[1] - orderedSides[2] * orderedSides[2]) < 1e-10;
        }

        public double CalculateSquare()
        {
            double p = (A + B + C) / 2;
            return Math.Sqrt(p * (p - A) * (p - B) * (p - C));
        }
    }
}
