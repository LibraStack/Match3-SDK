using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;

public class AnimatedItemSwapper : IItemSwapper
{
    private const float SwapDuration = 0.2f;

    public async UniTask SwapItemsAsync(IItem item1, IItem item2)
    {
        var item1Position = item1.GetWorldPosition();
        var item2Position = item2.GetWorldPosition();

        await DOTween.Sequence()
            .Join(item1.Transform.DOMove(item2Position, SwapDuration))
            .Join(item2.Transform.DOMove(item1Position, SwapDuration))
            .SetEase(Ease.Flash);

        item1.SetWorldPosition(item2Position);
        item2.SetWorldPosition(item1Position);
    }
}