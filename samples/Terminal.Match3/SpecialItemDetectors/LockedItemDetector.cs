using System.Collections.Generic;
using Match3.App.Interfaces;
using Terminal.Match3.Enums;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3.SpecialItemDetectors
{
    public class LockedItemDetector : ISpecialItemDetector<ITerminalGridSlot>
    {
        private readonly ITerminalGameBoardRenderer _gameBoardRenderer;

        public LockedItemDetector(ITerminalGameBoardRenderer gameBoardRenderer)
        {
            _gameBoardRenderer = gameBoardRenderer;
        }

        public IEnumerable<ITerminalGridSlot> GetSpecialItemGridSlots(IGameBoard<ITerminalGridSlot> gameBoard,
            ITerminalGridSlot gridSlot)
        {
            if (_gameBoardRenderer.GetTileGroup(gridSlot.GridPosition) != TileGroup.Locked)
            {
                yield break;
            }

            var hasNextState = _gameBoardRenderer.TrySetNextTileState(gridSlot.GridPosition);
            if (hasNextState)
            {
                yield break;
            }

            yield return gridSlot;
        }
    }
}