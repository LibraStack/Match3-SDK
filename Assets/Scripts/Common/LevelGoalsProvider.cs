using Common.Interfaces;
using Common.LevelGoals;
using Match3.Core;
using Match3.Core.Interfaces;

namespace Common
{
    public class LevelGoalsProvider : ILevelGoalsProvider<IUnityGridSlot>
    {
        public LevelGoal<IUnityGridSlot>[] GetLevelGoals(int level, IGameBoard<IUnityGridSlot> gameBoard)
        {
            return new LevelGoal<IUnityGridSlot>[] { new CollectRowMaxItems(gameBoard) };
        }
    }
}