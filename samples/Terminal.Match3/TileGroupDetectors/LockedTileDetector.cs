using Terminal.Match3.Enums;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3.TileGroupDetectors
{
    public class LockedTileDetector : ITileDetector<ITerminalGridSlot>
    {
        private readonly ITerminalGameBoardRenderer _gameBoardRenderer;

        public LockedTileDetector(ITerminalGameBoardRenderer gameBoardRenderer)
        {
            _gameBoardRenderer = gameBoardRenderer;
        }

        public void CheckGridSlot(ITerminalGridSlot gridSlot)
        {
            if (_gameBoardRenderer.GetTileGroup(gridSlot.GridPosition) == TileGroup.Locked)
            {
                _gameBoardRenderer.TrySetNextTileState(gridSlot.GridPosition);
            }
        }
    }
}