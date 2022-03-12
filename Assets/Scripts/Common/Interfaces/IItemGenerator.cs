namespace Common.Interfaces
{
    public interface IItemGenerator
    {
        IItem GetItem();
        void ReturnItem(IItem item);
    }
}