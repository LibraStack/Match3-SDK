namespace Match3.Core.Interfaces
{
    public interface IItemGenerator<TItem>
    {
        TItem GetItem();
        void ReturnItem(TItem item);
    }
}