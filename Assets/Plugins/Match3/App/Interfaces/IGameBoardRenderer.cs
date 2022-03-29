using System;
using Match3.Core.Structs;
using UnityEngine;

namespace Match3.App.Interfaces
{
    public interface IGameBoardRenderer : IDisposable
    {
        void CreateGridTiles();
        bool IsTileActive(GridPosition gridPosition);
        void ActivateTile(GridPosition gridPosition);
        void DeactivateTile(GridPosition gridPosition);

        bool IsPointerOnGrid(Vector3 worldPointerPosition, out GridPosition gridPosition);
        bool IsPointerOnBoard(Vector3 worldPointerPosition, out GridPosition gridPosition);

        Vector3 GetWorldPosition(GridPosition gridPosition);
        Vector3 GetWorldPosition(int rowIndex, int columnIndex);
    }
}