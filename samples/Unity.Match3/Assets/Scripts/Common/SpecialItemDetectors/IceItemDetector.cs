using System.Collections.Generic;
using Common.Enums;
using Common.Interfaces;
using Match3.App.Interfaces;

namespace Common.SpecialItemDetectors
{
    public class IceItemDetector : ISpecialItemDetector<IUnityGridSlot>
    {
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        public IceItemDetector(IUnityGameBoardRenderer gameBoardRenderer)
        {
            _gameBoardRenderer = gameBoardRenderer;
        }

        public IEnumerable<IUnityGridSlot> GetSpecialItemGridSlots(IGameBoard<IUnityGridSlot> gameBoard,
            IUnityGridSlot gridSlot)
        {
            if (_gameBoardRenderer.GetTileGroup(gridSlot.GridPosition) != TileGroup.Ice)
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