namespace Match3.Infrastructure.Interfaces
{
    public interface IItemsPool<TItem>
    {
        TItem GetItem();
        void ReturnItem(TItem item);
    }
}