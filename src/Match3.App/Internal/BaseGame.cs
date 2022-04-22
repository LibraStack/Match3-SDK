using System;
using Match3.App.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Structs;

namespace Match3.App.Internal
{
    public abstract class BaseGame<TGridSlot> : IDisposable where TGridSlot : IGridSlot
    {
        private readonly GameBoard<TGridSlot> _gameBoard;
        private readonly IGameBoardSolver<TGridSlot> _gameBoardSolver;
        private readonly ILevelGoalsProvider<TGridSlot> _levelGoalsProvider;
        private readonly IGameBoardDataProvider<TGridSlot> _gameBoardDataProvider;
        private readonly ISolvedSequencesConsumer<TGridSlot>[] _solvedSequencesConsumers;

        private bool _isStarted;
        private int _achievedGoals;

        private LevelGoal<TGridSlot>[] _levelGoals;

        protected BaseGame(GameConfig<TGridSlot> config)
        {
            _gameBoard = new GameBoard<TGridSlot>();

            _gameBoardSolver = config.GameBoardSolver;
            _levelGoalsProvider = config.LevelGoalsProvider;
            _gameBoardDataProvider = config.GameBoardDataProvider;
            _solvedSequencesConsumers = config.SolvedSequencesConsumers;
        }

        protected IGameBoard<TGridSlot> GameBoard => _gameBoard;

        public event EventHandler Finished;
        public event EventHandler<LevelGoal<TGridSlot>> LevelGoalAchieved;

        public void InitGameLevel(int level)
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Can not be initialized while the current game is active.");
            }

            _gameBoard.SetGridSlots(_gameBoardDataProvider.GetGameBoardSlots(level));
            _levelGoals = _levelGoalsProvider.GetLevelGoals(level, _gameBoard);
        }

        protected void StartGame()
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Game has already been started.");
            }

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.Achieved += OnLevelGoalAchieved;
            }

            _isStarted = true;
            OnGameStarted();
        }

        protected void StopGame()
        {
            if (_isStarted == false)
            {
                throw new InvalidOperationException("Game has not been started.");
            }

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.Achieved -= OnLevelGoalAchieved;
            }

            _isStarted = false;
            OnGameStopped();
        }

        public void ResetGameBoard()
        {
            _achievedGoals = 0;
            _gameBoard.ResetState();
        }

        public void Dispose()
        {
            _gameBoard?.Dispose();
        }

        protected abstract void OnGameStarted();
        protected abstract void OnGameStopped();

        protected bool IsSolved(GridPosition position1, GridPosition position2, out SolvedData<TGridSlot> solvedData)
        {
            solvedData = _gameBoardSolver.Solve(GameBoard, position1, position2);
            return solvedData.SolvedSequences.Count > 0;
        }

        protected void NotifySequencesSolved(SolvedData<TGridSlot> solvedData)
        {
            foreach (var sequencesConsumer in _solvedSequencesConsumers)
            {
                sequencesConsumer.OnSequencesSolved(solvedData);
            }

            foreach (var levelGoal in _levelGoals)
            {
                if (levelGoal.IsAchieved == false)
                {
                    levelGoal.OnSequencesSolved(solvedData);
                }
            }
        }

        protected virtual void OnAllGoalsAchieved()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private void OnLevelGoalAchieved(object sender, EventArgs e)
        {
            LevelGoalAchieved?.Invoke(this, (LevelGoal<TGridSlot>) sender);

            _achievedGoals++;
            if (_achievedGoals == _levelGoals.Length)
            {
                OnAllGoalsAchieved();
            }
        }
    }
}