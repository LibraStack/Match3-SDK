using Common.Enums;
using Common.Interfaces;
using Common.Structs;
using UnityEngine;

namespace Common.Models
{
    public class GridSlot
    {
        public IItem Item { get; private set; }
        public Vector3 WorldPosition { get; }
        public GridPosition GridPosition { get; }
        public GridSlotState State { get; private set; }

        public GridSlot(GridSlotState state, GridPosition gridPosition, Vector3 worldPosition)
        {
            State = state;
            GridPosition = gridPosition;
            WorldPosition = worldPosition;
        }

        public void Lock()
        {
            Debug.Assert(Item == null, "Can not lock a grid slot while it has an item.");

            State = GridSlotState.NotAvailable;
        }

        public void Unlock()
        {
            Debug.Assert(State == GridSlotState.NotAvailable, "Can not change {State} state to {GridSlotState.Free}.");

            State = GridSlotState.Free;
        }

        public void SetItem(IItem item)
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

            Debug.Assert(State == GridSlotState.Occupied, "Can not mark an unoccupied grid slot as solved.");

            State = GridSlotState.Solved;
        }

        public void Clear()
        {
            Debug.Assert(State != GridSlotState.NotAvailable, "Can not clear an unavailable grid slot.");

            Item = null;
            State = GridSlotState.Free;
        }
    }
}