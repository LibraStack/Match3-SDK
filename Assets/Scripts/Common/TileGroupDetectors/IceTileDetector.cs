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
            _gameBoardRenderer.TrySetNextTileState(gridSlot.GridPosition);
        }
    }
}