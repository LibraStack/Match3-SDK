using Cysharp.Threading.Tasks;
using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface IItemSwapper<in TItem> where TItem : IItem
    {
        UniTask SwapItemsAsync(TItem item1, TItem item2);
    }
}