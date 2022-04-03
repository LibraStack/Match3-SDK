using Match3.Core.Helpers;
using Match3.Core.Interfaces;
using Match3.Core.Structs;
using NSubstitute;
using NUnit.Framework;

namespace Match3.Tests
{
    public class GridMathTests
    {
        [Test]
        public void IsPositionOnGrid_ShouldReturnTrue_WhenPositionOnGrid()
        {
            // Arrange
            var grid = CreateGrid(5, 5);
            var gridPosition = GridPosition.Zero;

            // Act
            var result = GridMath.IsPositionOnGrid(grid, gridPosition);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void IsPositionOnGrid_ShouldReturnFalse_WhenPositionOutOfGrid()
        {
            // Arrange
            var grid = CreateGrid(5, 5);
            var gridPosition = new GridPosition(5, 5);

            // Act
            var result = GridMath.IsPositionOnGrid(grid, gridPosition);

            // Assert
            Assert.IsFalse(result);
        }

        private IGrid CreateGrid(int rowCount, int columnCount)
        {
            var grid = Substitute.For<IGrid>();
            grid.RowCount.Returns(rowCount);
            grid.ColumnCount.Returns(columnCount);

            return grid;
        }
    }
}
