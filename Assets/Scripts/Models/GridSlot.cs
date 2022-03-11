using Enums;
using Interfaces;
using UnityEngine;

namespace Models
{
    public class GridSlot
    {
        public IItem Item { set; get; }
        public Vector3 WorldPosition { get; }
        public GridPosition GridPosition { get; }
        public GridSlotState State { get; set; }
        
        public GridSlot(GridSlotState state, GridPosition gridPosition, Vector3 worldPosition)
        {
            State = state;
            GridPosition = gridPosition;
            WorldPosition = worldPosition;
        }
    }
}