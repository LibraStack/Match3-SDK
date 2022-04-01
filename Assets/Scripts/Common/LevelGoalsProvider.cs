using Common.Interfaces;
using Common.LevelGoals;
using Match3.App;
using Match3.App.Interfaces;

namespace Common
{
    public class LevelGoalsProvider : ILevelGoalsProvider<IUnityItem>
    {
        public LevelGoal<IUnityItem>[] GetLevelGoals(int level, IGameBoard<IUnityItem> gameBoard)
        {
            return new LevelGoal<IUnityItem>[] { new CollectRowMaxItems(gameBoard) };
        }
    }
}