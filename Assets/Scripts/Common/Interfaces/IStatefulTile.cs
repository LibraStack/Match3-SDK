namespace Common.Interfaces
{
    public interface IStatefulTile
    {
        bool NextState();
        void ResetState();
    }
}