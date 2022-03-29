using Match3.Core.Interfaces;

namespace Common.Interfaces
{
    public interface IItemsPool<TItem> where TItem : IItem
    {
        TItem GetItem();
        void ReturnItem(TItem item);
    }
}