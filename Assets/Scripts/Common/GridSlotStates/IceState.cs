using Common.Interfaces;
using Match3.Core.Interfaces;

namespace Common.GridSlotStates
{
    public class IceState : IGridSlotState, IStatefulSlot
    {
        private bool _isLocked = true;

        public bool IsLocked => _isLocked;
        public bool CanContainItem => true;

        public bool NextState()
        {
            _isLocked = false;
            return false;
        }

        public void ResetState()
        {
            _isLocked = true;
        }
    }
}