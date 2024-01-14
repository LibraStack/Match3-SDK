using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Match3.App.Interfaces;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3
{
    public class TerminalItemSwapper : IItemSwapper<ITerminalGridSlot>
    {
        private const int SwapDelay = 250;

        private readonly ITerminalGameBoardRenderer _gameBoardRenderer;

        public TerminalItemSwapper(ITerminalGameBoardRenderer gameBoardRenderer)
        {
            _gameBoardRenderer = gameBoardRenderer;
        }

        public async UniTask SwapItemsAsync(ITerminalGridSlot gridSlot1, ITerminalGridSlot gridSlot2,
            CancellationToken cancellationToken = default)
        {
            var item1 = gridSlot1.Item;
            var item2 = gridSlot2.Item;

            gridSlot1.SetItem(item2);
            gridSlot2.SetItem(item1);

            _gameBoardRenderer.RedrawGameBoard();

            await Task.Delay(SwapDelay, cancellationToken);
        }
    }
}