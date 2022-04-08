using Match3.Core.Interfaces;

namespace Common.GridSlotStates
{
    public class AvailableState : IGridSlotState
    {
        public bool IsLocked => false;
        public bool CanContainItem => true;
    }
}