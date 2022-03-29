using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface ILevelGoalsProvider<TItem> where TItem : IItem
    {
        LevelGoal<TItem>[] GetLevelGoals(int level, IGameBoard<TItem> gameBoard);
    }
}