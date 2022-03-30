using System.Runtime.CompilerServices;
using DG.Tweening;
using FillStrategies.Models;
using Match3.App;

namespace FillStrategies.Jobs
{
    public abstract class DropJob : Job
    {
        private const float MoveDuration = 0.25f;

        protected DropJob(int executionOrder) : base(executionOrder)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Tween CreateItemMoveTween(ItemMoveData data)
        {
            return data.Item.Transform.DOPath(data.WorldPositions, MoveDuration);
        }
    }
}