using Cysharp.Threading.Tasks;

namespace Match3.Core.Interfaces
{
    public interface IItemSwapper
    {
        UniTask SwapItemsAsync(IItem item1, IItem item2);
    }
}