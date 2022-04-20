using Common.Enums;
using Match3.Core.Structs;
using Match3.Template.Interfaces;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IUnityGameBoardRenderer : IGameBoardRenderer
    {
        bool IsTileActive(GridPosition gridPosition);
        void ActivateTile(GridPosition gridPosition);
        void DeactivateTile(GridPosition gridPosition);
        void SetNextGridTileGroup(GridPosition gridPosition);

        bool IsPositionOnGrid(GridPosition gridPosition);
        bool IsPointerOnGrid(Vector3 worldPointerPosition, out GridPosition gridPosition);
        bool IsPointerOnBoard(Vector3 worldPointerPosition, out GridPosition gridPosition);

        Vector3 GetWorldPosition(GridPosition gridPosition);

        bool TrySetNextTileState(GridPosition gridPosition);
        TileGroup GetTileGroup(GridPosition gridPosition);
    }
}