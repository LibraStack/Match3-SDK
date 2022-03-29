using System;
using Common.Interfaces;
using Match3.App;
using Match3.App.Interfaces;

namespace Common.AppModes
{
    public class GamePlayMode : IAppMode, IDeactivatable
    {
        private readonly IGameUiCanvas _gameUiCanvas;
        private readonly Match3Game<IUnityItem> _match3Game;
        private readonly IBoardFillStrategy<IUnityItem>[] _boardFillStrategies;

        public GamePlayMode(IAppContext appContext)
        {
            _gameUiCanvas = appContext.Resolve<IGameUiCanvas>();
            _match3Game = appContext.Resolve<Match3Game<IUnityItem>>();
            _boardFillStrategies = appContext.Resolve<IBoardFillStrategy<IUnityItem>[]>();
        }

        public event EventHandler Finished
        {
            add => _match3Game.Finished += value;
            remove => _match3Game.Finished -= value;
        }

        public void Activate()
        {
            _match3Game.LevelGoalAchieved += OnLevelGoalAchieved;
            _gameUiCanvas.StrategyChanged += OnStrategyChanged;

            _match3Game.SetGameBoardFillStrategy(GetSelectedFillStrategy());
            _match3Game.Start(0);
        }

        public void Deactivate()
        {
            _match3Game.LevelGoalAchieved -= OnLevelGoalAchieved;
            _gameUiCanvas.StrategyChanged -= OnStrategyChanged;

            _match3Game.Stop();
        }

        private void OnLevelGoalAchieved(object sender, LevelGoal<IUnityItem> levelGoal)
        {
            _gameUiCanvas.RegisterAchievedGoal(levelGoal);
        }

        private void OnStrategyChanged(object sender, int index)
        {
            _match3Game.SetGameBoardFillStrategy(GetFillStrategy(index));
        }

        private IBoardFillStrategy<IUnityItem> GetSelectedFillStrategy()
        {
            return GetFillStrategy(_gameUiCanvas.SelectedFillStrategyIndex);
        }

        private IBoardFillStrategy<IUnityItem> GetFillStrategy(int index)
        {
            return _boardFillStrategies[index];
        }
    }
}