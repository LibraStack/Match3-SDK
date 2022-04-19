namespace Match3.Template.Interfaces
{
    public interface IItemsPool<TItem>
    {
        TItem GetItem();
        void ReturnItem(TItem item);
    }
}