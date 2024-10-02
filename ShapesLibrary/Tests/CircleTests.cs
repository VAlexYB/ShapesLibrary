using ShapesLibrary.Interfaces;
using ShapesLibrary.Shapes;
using Xunit;

namespace ShapesLibrary.Tests
{
    public class CircleTests
    {
        [Fact]
        public void CircleSquare_ShouldReturnCorrectValue()
        {
            IShape circle = new Circle(5);
            var square = circle.CalculateSquare();
            Assert.Equal(Math.PI * 5 * 5, square, 1e-10);
        }
    }
}
