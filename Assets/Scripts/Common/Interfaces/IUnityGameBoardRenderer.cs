using Match3.App.Interfaces;
using Match3.Core.Structs;

namespace Common.Interfaces
{
    public interface IUnityGameBoardRenderer : IGameBoardRenderer
    {
        void CreateGridTiles();
        bool IsTileActive(GridPosition gridPosition);
        void ActivateTile(GridPosition gridPosition);
        void DeactivateTile(GridPosition gridPosition);
        void SetNextGridTileGroup(GridPosition gridPosition);
        void TrySetNextTileState(GridPosition gridPosition);
    }
}