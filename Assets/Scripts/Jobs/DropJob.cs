using Cysharp.Threading.Tasks;
using DG.Tweening;
using Interfaces;
using Models;

namespace Jobs
{
    public abstract class DropJob : IJob
    {
        private const float MoveDuration = 0.25f;

        public abstract UniTask ExecuteAsync();

        protected Sequence CreateItemMoveSequence(ItemDropData itemDropData)
        {
            var dropSequence = DOTween.Sequence();
            var moveDuration = MoveDuration / itemDropData.Positions.Count;

            foreach (var position in itemDropData.Positions)
            {
                dropSequence.Append(itemDropData.Item.Transform.DOMove(position, moveDuration));
            }

            return dropSequence;
        }
    }
}