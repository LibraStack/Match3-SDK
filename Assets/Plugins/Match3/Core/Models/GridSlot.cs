using System.Diagnostics;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.Core.Models
{
    public class GridSlot<TItem> where TItem : IItem
    {
        public TItem Item { get; private set; }
        public GridPosition GridPosition { get; }
        public GridSlotState State { get; private set; }

        internal GridSlot(GridSlotState state, GridPosition gridPosition)
        {
            State = state;
            GridPosition = gridPosition;
        }

        public void SetItem(TItem item)
        {
            Item = item;
            State = GridSlotState.Occupied;
        }

        public void MarkSolved()
        {
            if (State == GridSlotState.Solved)
            {
                return;
            }

            Assert(State == GridSlotState.Occupied, "Can not mark an unoccupied grid slot as solved.");

            State = GridSlotState.Solved;
        }

        public void Clear()
        {
            Assert(State != GridSlotState.NotAvailable, "Can not clear an unavailable grid slot.");

            Item = default;
            State = GridSlotState.Empty;
        }

        [Conditional("DEBUG")]
        private void Assert(bool condition, string message)
        {
            UnityEngine.Debug.Assert(condition, message);
        }
    }
}