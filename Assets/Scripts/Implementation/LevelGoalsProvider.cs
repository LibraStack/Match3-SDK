using Implementation.Common.Interfaces;
using Implementation.LevelGoals;
using Match3.Core.Interfaces;

namespace Implementation
{
    public class LevelGoalsProvider : ILevelGoalsProvider
    {
        public ILevelGoal[] GetLevelGoals(IGameBoard<IUnityItem> gameBoard)
        {
            return new ILevelGoal[] { new CollectRowMaxItems(gameBoard) };
        }
    }
}