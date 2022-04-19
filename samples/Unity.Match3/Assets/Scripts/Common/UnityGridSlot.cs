using System;
using System.Runtime.CompilerServices;
using Common.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Common
{
    public class UnityGridSlot : IUnityGridSlot
    {
        public UnityGridSlot(IGridSlotState state, GridPosition gridPosition)
        {
            State = state;
            GridPosition = gridPosition;
        }

        public int ItemId => Item.ContentId;

        public bool HasItem => Item != null;
        public bool IsMovable => State.IsLocked == false && HasItem;
        public bool CanContainItem => State.CanContainItem;
        public bool CanSetItem => State.CanContainItem && HasItem == false;
        public bool NotAvailable => State.CanContainItem == false || State.IsLocked;

        public IUnityItem Item { get; private set; }
        public IGridSlotState State { get; private set; }
        public GridPosition GridPosition { get; }

        public void SetState(IGridSlotState state)
        {
            State = state;
        }

        public void SetItem(IUnityItem item)
        {
            EnsureItemIsNotNull(item);

            Item = item;
        }

        public void Clear()
        {
            if (State.CanContainItem == false)
            {
                throw new InvalidOperationException("Can not clear an unavailable grid slot.");
            }

            Item = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureItemIsNotNull(IUnityItem item)
        {
            if (item == null)
            {
                throw new NullReferenceException(nameof(item));
            }
        }
    }
}