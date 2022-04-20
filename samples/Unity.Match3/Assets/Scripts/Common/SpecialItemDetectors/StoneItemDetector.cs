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
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        public StoneItemDetector(IUnityGameBoardRenderer gameBoardRenderer)
        {
            _gameBoardRenderer = gameBoardRenderer;
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

                if (!_gameBoardRenderer.IsPositionOnGrid(position) ||
                    _gameBoardRenderer.GetTileGroup(position) != TileGroup.Stone)
                {
                    continue;
                }

                var hasNextState = _gameBoardRenderer.TrySetNextTileState(position);
                if (hasNextState)
                {
                    continue;
                }

                yield return gameBoard[position];
            }
        }
    }
}