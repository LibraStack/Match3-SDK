namespace Common.Interfaces
{
    public interface IItemsPool<TUnityItem>
    {
        TUnityItem GetItem();
        void ReturnItem(TUnityItem item);
    }
}