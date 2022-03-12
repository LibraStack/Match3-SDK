using Models;
using UnityEngine;

namespace Interfaces
{
    public interface IGrid
    {
        int RowCount { get; }
        int ColumnCount { get; }

        GridSlot this[int rowIndex, int columnIndex] { get; }

        bool IsPositionOnBoard(GridPosition gridPosition);
        bool IsPointerOnGrid(Vector3 worldPointerPosition, out GridPosition gridPosition);
        bool IsPointerOnBoard(Vector3 worldPointerPosition, out GridPosition gridPosition);
        Vector3 GetWorldPosition(int rowIndex, int columnIndex);
    }
}