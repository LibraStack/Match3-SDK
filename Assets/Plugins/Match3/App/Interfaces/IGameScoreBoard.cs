using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface IGameScoreBoard<TItem> : ISolvedSequencesConsumer<TItem> where TItem : IItem
    {
    }
}