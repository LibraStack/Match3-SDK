using Match3.Core.Interfaces;

namespace Implementation.Common.Interfaces
{
    public interface ILevelGoalsProvider
    {
        ILevelGoal[] GetLevelGoals(IGameBoard<IUnityItem> gameBoard);
    }
}