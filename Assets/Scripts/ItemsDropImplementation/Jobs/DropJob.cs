using Common.Interfaces;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ItemsDropImplementation.Models;

namespace ItemsDropImplementation.Jobs
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