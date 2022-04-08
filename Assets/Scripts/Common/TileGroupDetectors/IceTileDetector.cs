using Common.Enums;
using Common.Interfaces;
using Match3.Core.Models;

namespace Common.TileGroupDetectors
{
    public class IceTileDetector : ITileDetector
    {
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        public IceTileDetector(IUnityGameBoardRenderer gameBoardRenderer)
        {
            _gameBoardRenderer = gameBoardRenderer;
        }

        public void CheckGridSlot(GridSlot<IUnityItem> gridSlot)
        {
            if (_gameBoardRenderer.GetTileGroup(gridSlot.GridPosition) == TileGroup.Ice)
            {
                _gameBoardRenderer.TrySetNextTileState(gridSlot.GridPosition);
            }
        }
    }
}