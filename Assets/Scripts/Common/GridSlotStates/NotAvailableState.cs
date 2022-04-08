using Match3.Core.Interfaces;

namespace Common.GridSlotStates
{
    public class NotAvailableState : IGridSlotState
    {
        public bool IsLocked => true;
        public bool CanContainItem => false;
    }
}