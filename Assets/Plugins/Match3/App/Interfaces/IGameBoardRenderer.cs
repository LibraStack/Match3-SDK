using System;
using Match3.Core.Structs;
using UnityEngine;

namespace Match3.App.Interfaces
{
    public interface IGameBoardRenderer : IDisposable
    {
        bool IsPointerOnGrid(Vector3 worldPointerPosition, out GridPosition gridPosition);
        bool IsPointerOnBoard(Vector3 worldPointerPosition, out GridPosition gridPosition);
        bool IsInteractableSlot(GridPosition gridPosition);

        Vector3 GetWorldPosition(GridPosition gridPosition);

        void ResetState();
    }
}