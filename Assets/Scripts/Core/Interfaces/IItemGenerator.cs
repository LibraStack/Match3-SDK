namespace Match3.Core.Interfaces
{
    public interface IItemGenerator<TItem> where TItem : IItem
    {
        TItem GetItem();
        void ReturnItem(TItem item);
    }
}