using Cysharp.Threading.Tasks;

namespace Match3.UnityApp.Interfaces
{
    public interface IItemSwapper<in TGridSlot>
    {
        UniTask SwapItemsAsync(TGridSlot gridSlot1, TGridSlot gridSlot2);
    }
}