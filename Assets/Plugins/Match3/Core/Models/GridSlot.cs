using System;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.Core.Models
{
    public sealed class GridSlot<TItem> where TItem : IItem
    {
        public bool HasItem => Item != null;
        public bool IsMovable => State.IsLocked == false && HasItem;
        public bool CanSetItem => State.CanContainItem && HasItem == false;
        public bool NotAvailable => State.CanContainItem == false || State.IsLocked;

        public TItem Item { get; private set; }
        public IGridSlotState State { get; private set; }
        public GridPosition GridPosition { get; }

        public GridSlot(IGridSlotState state, GridPosition gridPosition)
        {
            State = state;
            GridPosition = gridPosition;
        }

        public void SetState(IGridSlotState state)
        {
            State = state;
        }

        public void SetItem(TItem item)
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

        private void EnsureItemIsNotNull(TItem item, string message = default)
        {
            if (item == null)
            {
                throw new NullReferenceException(string.IsNullOrEmpty(message) ? nameof(item) : message);
            }
        }
    }
}