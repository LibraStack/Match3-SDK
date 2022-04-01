using System.Collections.Generic;
using Common.Interfaces;
using Common.LevelGoals;
using Match3.App;
using Match3.App.Interfaces;

namespace Common
{
    public class LevelGoalsProvider : ILevelGoalsProvider<IUnityItem>
    {
        // private Dictionary<int, LevelGoal<IUnityItem>[]> _levelGoals;
        //
        // public LevelGoalsProvider()
        // {
        //     _levelGoals = new Dictionary<int, LevelGoal<IUnityItem>[]>
        //     {
        //         { 0, new LevelGoal<IUnityItem>[] { new CollectRowMaxItems(gameBoard) } }
        //     };
        // }
        
        public LevelGoal<IUnityItem>[] GetLevelGoals(int level, IGameBoard<IUnityItem> gameBoard)
        {
            return new LevelGoal<IUnityItem>[] { new CollectRowMaxItems(gameBoard) };
        }
    }
}