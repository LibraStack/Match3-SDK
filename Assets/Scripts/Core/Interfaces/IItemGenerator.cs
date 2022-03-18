namespace Match3.Core.Interfaces
{
    public interface IItemGenerator
    {
        IItem GetItem();
        void ReturnItem(IItem item);
    }
}