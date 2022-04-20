namespace Common.Interfaces
{
    public interface IStatefulSlot
    {
        void NextState();
        void ResetState();
    }
}