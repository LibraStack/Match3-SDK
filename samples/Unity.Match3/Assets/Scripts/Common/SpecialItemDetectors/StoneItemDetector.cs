using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.Core.Structs;

namespace Common.SpecialItemDetectors
{
    public class StoneItemDetector : ISpecialItemDetector<IUnityGridSlot>
    {
        private readonly GridPosition[] _lookupDirections;

        public StoneItemDetector()
        {
            _lookupDirections = new[]
            {
                GridPosition.Up,
                GridPosition.Down,
                GridPosition.Left,
                GridPosition.Right
            };
        }

        public IEnumerable<IUnityGridSlot> GetSpecialItemGridSlots(IGameBoard<IUnityGridSlot> gameBoard,
            IUnityGridSlot gridSlot)
        {
            if (gridSlot.IsMovable == false)
            {
                yield break;
            }

            foreach (var lookupDirection in _lookupDirections)
            {
                var position = gridSlot.GridPosition + lookupDirection;
                if (gameBoard.IsPositionOnGrid(position) == false)
                {
                    continue;
                }

                var lookupGridSlot = gameBoard[position];
                if (lookupGridSlot.State.GroupId == (int) TileGroup.Stone)
                {
                    yield return lookupGridSlot;
                }
            }
        }
    }
}