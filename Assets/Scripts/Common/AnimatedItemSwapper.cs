using Common.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Match3.App.Interfaces;

namespace Common
{
    public class AnimatedItemSwapper : IItemSwapper<IUnityItem>
    {
        private const float SwapDuration = 0.2f;

        public async UniTask SwapItemsAsync(IUnityItem item1, IUnityItem item2)
        {
            var item1WorldPosition = item1.GetWorldPosition();
            var item2WorldPosition = item2.GetWorldPosition();

            await DOTween.Sequence()
                .Join(item1.Transform.DOMove(item2WorldPosition, SwapDuration))
                .Join(item2.Transform.DOMove(item1WorldPosition, SwapDuration))
                .SetEase(Ease.Flash);

            item1.SetWorldPosition(item2WorldPosition);
            item2.SetWorldPosition(item1WorldPosition);
        }
    }
}