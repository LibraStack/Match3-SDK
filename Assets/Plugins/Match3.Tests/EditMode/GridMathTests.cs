using System.Collections.Generic;
using Match3.Core.Helpers;
using Match3.Core.Interfaces;
using Match3.Core.Structs;
using NSubstitute;
using NUnit.Framework;

namespace Match3.Tests.EditMode
{
    public class GridMathTests
    {
        [TestCaseSource(nameof(TestDataCases))]
        public void IsPositionOnGrid(int rowCount, int columnCount, GridPosition gridPosition, bool expectedResult)
        {
            // Arrange
            var grid = Substitute.For<IGrid>();
            grid.RowCount.Returns(rowCount);
            grid.ColumnCount.Returns(columnCount);

            // Act
            var result = GridMath.IsPositionOnGrid(grid, gridPosition);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        private static IEnumerable<object[]> TestDataCases()
        {
            yield return new object[] { 5, 5, new GridPosition(0, 0), true };
            yield return new object[] { 5, 5, new GridPosition(4, 4), true };
            yield return new object[] { 5, 5, new GridPosition(0, 5), false };
            yield return new object[] { 5, 5, new GridPosition(5, 0), false };
            yield return new object[] { 5, 5, new GridPosition(5, 5), false };
            yield return new object[] { 5, 5, new GridPosition(-5, 4), false };
        }
    }
}
