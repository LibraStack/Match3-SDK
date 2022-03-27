using Implementation.Common.LevelGoals;
using Match3.Core.Interfaces;

namespace Implementation.Common.Interfaces
{
    public interface ILevelGoalsProvider
    {
        LevelGoal[] GetLevelGoals(IGameBoard<IUnityItem> gameBoard);
    }
}