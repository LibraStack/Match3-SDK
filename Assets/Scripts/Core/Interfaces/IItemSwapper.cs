using Cysharp.Threading.Tasks;

namespace Match3.Core.Interfaces
{
    public interface IItemSwapper<in TItem> where TItem : IItem
    {
        UniTask SwapItemsAsync(TItem item1, TItem item2);
    }
}