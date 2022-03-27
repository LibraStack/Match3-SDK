using Implementation.Common.Interfaces;
using Implementation.Common.LevelGoals;
using Match3.Core.Interfaces;

namespace Implementation.Common
{
    public class LevelGoalsProvider : ILevelGoalsProvider
    {
        public LevelGoal[] GetLevelGoals(IGameBoard<IUnityItem> gameBoard)
        {
            return new LevelGoal[] { new CollectRowMaxItems(gameBoard) };
        }
    }
}