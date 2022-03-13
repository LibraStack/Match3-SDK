using Common.Models;
using Common.Structs;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IGrid
    {
        int RowCount { get; }
        int ColumnCount { get; }

        GridSlot this[GridPosition gridPosition] { get; }
        GridSlot this[int rowIndex, int columnIndex] { get; }

        bool IsPositionOnGrid(GridPosition gridPosition);
        bool IsPositionOnBoard(GridPosition gridPosition);
        bool IsPointerOnGrid(Vector3 worldPointerPosition, out GridPosition gridPosition);
        bool IsPointerOnBoard(Vector3 worldPointerPosition, out GridPosition gridPosition);
        Vector3 GetWorldPosition(int rowIndex, int columnIndex);
    }
}