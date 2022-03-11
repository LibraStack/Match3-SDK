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
        bool IsPositionOnBoard(Vector3 worldPosition, out GridPosition gridPosition);
        GridPosition GetGridPosition(Vector3 worldPosition);
        Vector3 GetWorldPosition(int rowIndex, int columnIndex);
    }
}