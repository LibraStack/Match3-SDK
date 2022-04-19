using System.Threading;
using Cysharp.Threading.Tasks;
using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface IItemSwapper<in TGridSlot> where TGridSlot : IGridSlot
    {
        UniTask SwapItemsAsync(TGridSlot gridSlot1, TGridSlot gridSlot2, CancellationToken cancellationToken = default);
    }
}