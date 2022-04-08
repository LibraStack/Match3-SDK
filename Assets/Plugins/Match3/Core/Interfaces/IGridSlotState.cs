namespace Match3.Core.Interfaces
{
    public interface IGridSlotState
    {
        bool IsLocked { get; }
        bool CanContainItem { get; }
    }
}