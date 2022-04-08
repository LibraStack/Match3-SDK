using Common.Interfaces;
using Match3.Core.Interfaces;

namespace Common.GridSlotStates
{
    public class StoneState : IGridSlotState, IStatefulSlot
    {
        public bool IsLocked { get; private set; } = true;
        public bool CanContainItem { get; private set; }

        public bool NextState()
        {
            IsLocked = false;
            CanContainItem = true;

            return false;
        }

        public void ResetState()
        {
            IsLocked = true;
            CanContainItem = false;
        }
    }
}