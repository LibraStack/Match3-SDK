using Common.Enums;
using Common.Interfaces;

namespace Common.TileGroupDetectors
{
    public class IceTileDetector : ITileDetector
    {
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        public IceTileDetector(IUnityGameBoardRenderer gameBoardRenderer)
        {
            _gameBoardRenderer = gameBoardRenderer;
        }

        public void CheckGridSlot(IUnityGridSlot gridSlot)
        {
            if (_gameBoardRenderer.GetTileGroup(gridSlot.GridPosition) == TileGroup.Ice)
            {
                _gameBoardRenderer.TrySetNextTileState(gridSlot.GridPosition);
            }
        }
    }
}