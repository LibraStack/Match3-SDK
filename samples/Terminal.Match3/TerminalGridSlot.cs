using System;
using System.Runtime.CompilerServices;
using Match3.Core.Interfaces;
using Match3.Core.Structs;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3
{
    public sealed class TerminalGridSlot : ITerminalGridSlot
    {
        public TerminalGridSlot(IGridSlotState state, GridPosition gridPosition)
        {
            State = state;
            GridPosition = gridPosition;
        }

        public int ItemId => Item.ContentId;

        public bool HasItem => Item != null;
        public bool IsLocked => State.IsLocked;
        public bool CanContainItem => State.CanContainItem;
        public bool IsMovable => IsLocked == false && HasItem;
        public bool CanSetItem => CanContainItem && HasItem == false;

        public ITerminalItem Item { get; private set; }
        public IGridSlotState State { get; }
        public GridPosition GridPosition { get; }

        public void SetItem(ITerminalItem item)
        {
            EnsureItemIsNotNull(item);
            Item = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureItemIsNotNull(ITerminalItem item)
        {
            if (item == null)
            {
                throw new NullReferenceException(nameof(item));
            }
        }
    }
}