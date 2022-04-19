using System.Collections.Generic;
using Match3.App;
using Match3.App.Interfaces;
using Terminal.Match3.Interfaces;
using Terminal.Match3.Extensions;

namespace Terminal.Match3.TileGroupDetectors
{
    public class TileGroupDetector : ISolvedSequencesConsumer<ITerminalGridSlot>
    {
        private readonly ITileDetector<ITerminalGridSlot>[] _tileDetectors;

        public TileGroupDetector(ITerminalGameBoardRenderer gameBoardRenderer)
        {
            _tileDetectors = new ITileDetector<ITerminalGridSlot>[]
            {
                new LockedTileDetector(gameBoardRenderer)
            };
        }

        public void OnSequencesSolved(IEnumerable<ItemSequence<ITerminalGridSlot>> sequences)
        {
            foreach (var solvedGridSlot in sequences.GetUniqueGridSlots())
            {
                foreach (var tileDetector in _tileDetectors)
                {
                    tileDetector.CheckGridSlot(solvedGridSlot);
                }
            }
        }
    }
}