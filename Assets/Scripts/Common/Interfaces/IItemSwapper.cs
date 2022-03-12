using Cysharp.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IItemSwapper
    {
        UniTask SwapItemsAsync(IItem item1, IItem item2);
    }
}