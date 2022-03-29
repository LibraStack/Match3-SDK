namespace Match3.Core.Interfaces
{
    public interface IItem
    {
        int ContentId { get; }

        void Show();
        void Hide();
    }
}