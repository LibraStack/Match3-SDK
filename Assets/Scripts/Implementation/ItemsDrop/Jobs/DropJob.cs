using System.Linq;
using DG.Tweening;
using Implementation.ItemsDrop.Models;
using Match3.Core.Models;

namespace Implementation.ItemsDrop.Jobs
{
    public abstract class DropJob : Job
    {
        private const float MoveDuration = 0.25f;
        // private const float MoveDuration = 1.25f;

        protected DropJob(int executionOrder) : base(executionOrder)
        {
        }

        protected Sequence CreateItemMoveSequence(ItemMoveData itemDropData)
        {
            var dropSequence = DOTween.Sequence();
            var moveDuration = MoveDuration / itemDropData.Positions.Count();

            foreach (var position in itemDropData.Positions)
            {
                dropSequence.Append(itemDropData.Item.Transform.DOMove(position, moveDuration));
            }

            return dropSequence;
        }
    }
}