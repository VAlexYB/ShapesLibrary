using ShapesLibrary.Interfaces;
using ShapesLibrary.Shapes;
using Xunit;

namespace ShapesLibrary.Tests
{
    public class TriangleTests
    {
        [Fact]
        public void Triangle_InvalidSides_ShouldThrowException()
        {
            Assert.Throws<ArgumentException>(() => new Triangle(1, 1, 3));
        }

        [Fact]
        public void Triangle_ShouldBeRightAnged()
        {
            var triangle = new Triangle(3, 4, 5);
            Assert.True(triangle.IsRightAngled());
        }

        [Fact]
        public void TriangleSquare_ShouldReturnCorrectValue()
        {
            IShape triangle = new Triangle(3, 4, 5);
            var square = triangle.CalculateSquare();
            Assert.Equal(6, square, 1e-10);
        }
    }
}
