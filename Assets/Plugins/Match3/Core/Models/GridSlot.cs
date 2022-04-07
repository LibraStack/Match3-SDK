using System;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.Core.Models
{
    public class GridSlot<TItem> where TItem : IItem
    {
        public bool IsLocked { get; private set; }
        public TItem Item { get; private set; }
        public GridPosition GridPosition { get; }
        public GridSlotState State { get; private set; }

        internal GridSlot(GridSlotState state, GridPosition gridPosition)
        {
            State = state;
            GridPosition = gridPosition;
        }

        public void Lock()
        {
            IsLocked = true;
        }

        public void Unlock()
        {
            IsLocked = false;
        }

        public void SetItem(TItem item)
        {
            EnsureItemIsNotNull(item);

            Item = item;
            State = GridSlotState.Occupied;
        }

        public void MarkSolved()
        {
            EnsureItemIsNotNull(Item, "Can not mark an unoccupied grid slot as solved.");

            if (State == GridSlotState.Solved)
            {
                return;
            }

            State = GridSlotState.Solved;
        }

        public void Clear()
        {
            if (State == GridSlotState.NotAvailable)
            {
                throw new InvalidOperationException("Can not clear an unavailable grid slot.");
            }

            Item = default;
            State = GridSlotState.Empty;
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