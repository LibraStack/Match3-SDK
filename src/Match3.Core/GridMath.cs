using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.Core
{
    public static class GridMath
    {
        public static bool IsPositionOnGrid(IGrid grid, GridPosition gridPosition)
        {
            return IsPositionOnGrid(gridPosition, grid.RowCount, grid.ColumnCount);
        }

        public static bool IsPositionOnGrid(GridPosition gridPosition, int rowCount, int columnCount)
        {
            return gridPosition.RowIndex >= 0 &&
                   gridPosition.RowIndex < rowCount &&
                   gridPosition.ColumnIndex >= 0 &&
                   gridPosition.ColumnIndex < columnCount;
        }
    }
}