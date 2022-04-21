namespace Match3.Core.Interfaces
{
    public interface IStatefulSlot
    {
        bool NextState();
        void ResetState();
    }
}