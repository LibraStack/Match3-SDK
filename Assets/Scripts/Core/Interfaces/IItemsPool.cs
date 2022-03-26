namespace Match3.Core.Interfaces
{
    public interface IItemsPool<TItem> where TItem : IItem
    {
        TItem GetItem();
        void ReturnItem(TItem item);
    }
}