using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface ILevelGoalsProvider<TGridSlot> where TGridSlot : IGridSlot
    {
        LevelGoal<TGridSlot>[] GetLevelGoals(int level, IGameBoard<TGridSlot> gameBoard);
    }
}