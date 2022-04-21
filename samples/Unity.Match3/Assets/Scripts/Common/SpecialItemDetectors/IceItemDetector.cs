using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Match3.App.Interfaces;

namespace Common.SpecialItemDetectors
{
    public class IceItemDetector : ISpecialItemDetector<IUnityGridSlot>
    {
        public IEnumerable<IUnityGridSlot> GetSpecialItemGridSlots(IGameBoard<IUnityGridSlot> gameBoard,
            IUnityGridSlot gridSlot)
        {
            if (gridSlot.State.GroupId == (int) TileGroup.Ice)
            {
                yield return gridSlot;
            }
        }
    }
}