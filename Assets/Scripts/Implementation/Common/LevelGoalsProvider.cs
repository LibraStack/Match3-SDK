using Implementation.Common.Interfaces;
using Implementation.Common.LevelGoals;
using Match3.Core.Interfaces;

namespace Implementation.Common
{
    public class LevelGoalsProvider : ILevelGoalsProvider
    {
        public ILevelGoal[] GetLevelGoals(IGameBoard<IUnityItem> gameBoard)
        {
            return new ILevelGoal[] { new CollectRowMaxItems(gameBoard) };
        }
    }
}