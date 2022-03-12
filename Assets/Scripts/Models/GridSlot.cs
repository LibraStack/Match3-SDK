using Enums;
using Interfaces;
using UnityEngine;

namespace Models
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
            State = GridSlotState.NotAvailable;
        }

        public void Unlock()
        {
            State = GridSlotState.Free;
        }

        public void SetItem(IItem item)
        {
            Item = item;
            State = GridSlotState.Occupied;
        }

        public void MarkSolved()
        {
            State = GridSlotState.Solved;
        }

        public void Clear()
        {
            Item = null;
            State = GridSlotState.Free;
        }
    }
}