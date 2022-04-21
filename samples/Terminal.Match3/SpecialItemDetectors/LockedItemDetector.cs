using System.Collections.Generic;
using Match3.App.Interfaces;
using Terminal.Match3.Enums;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3.SpecialItemDetectors
{
    public class LockedItemDetector : ISpecialItemDetector<ITerminalGridSlot>
    {
        public IEnumerable<ITerminalGridSlot> GetSpecialItemGridSlots(IGameBoard<ITerminalGridSlot> gameBoard,
            ITerminalGridSlot gridSlot)
        {
            if (gridSlot.State.GroupId == (int) TileGroup.Locked)
            {
                yield return gridSlot;
            }
        }
    }
}