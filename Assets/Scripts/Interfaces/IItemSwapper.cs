using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IItemSwapper
    {
        UniTask SwapItemsAsync(IItem item1, IItem item2);
    }
}