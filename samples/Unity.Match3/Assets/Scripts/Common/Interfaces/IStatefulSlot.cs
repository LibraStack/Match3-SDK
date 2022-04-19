namespace Common.Interfaces
{
    public interface IStatefulSlot
    {
        bool NextState();
        void ResetState();
    }
}