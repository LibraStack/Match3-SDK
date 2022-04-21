namespace Match3.Core.Interfaces
{
    public interface IGridSlotState
    {
        int GroupId { get; }
        bool IsLocked { get; }
        bool CanContainItem { get; }
    }
}