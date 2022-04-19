using System.Threading;
using Common.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.App.Interfaces;

namespace Common
{
    public class AnimatedItemSwapper : IItemSwapper<IUnityGridSlot>
    {
        private const float SwapDuration = 0.2f;

        public async UniTask SwapItemsAsync(IUnityGridSlot gridSlot1, IUnityGridSlot gridSlot2,
            CancellationToken cancellationToken = default)
        {
            var item1 = gridSlot1.Item;
            var item2 = gridSlot2.Item;

            var item1WorldPosition = item1.GetWorldPosition();
            var item2WorldPosition = item2.GetWorldPosition();

            await DOTween.Sequence()
                .Join(item1.Transform.DOMove(item2WorldPosition, SwapDuration))
                .Join(item2.Transform.DOMove(item1WorldPosition, SwapDuration))
                .SetEase(Ease.Flash)
                .WithCancellation(cancellationToken);

            item1.SetWorldPosition(item2WorldPosition);
            item2.SetWorldPosition(item1WorldPosition);

            gridSlot1.SetItem(item2);
            gridSlot2.SetItem(item1);
        }
    }
}