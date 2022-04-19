using Match3.Core.Structs;

namespace Match3.Core.Interfaces
{
    public interface IGrid
    {
        int RowCount { get; }
        int ColumnCount { get; }

        bool IsPositionOnGrid(GridPosition gridPosition);
    }
}