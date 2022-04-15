using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.Core;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;
using Match3.UnityApp.Interfaces;
using Match3.UnityApp.Internal;

namespace Match3.UnityApp
{
    public abstract class Match3Game<TGridSlot> : IDisposable where TGridSlot : IGridSlot
    {
        private readonly GameBoard<TGridSlot> _gameBoard;
        private readonly ILevelGoalsProvider<TGridSlot> _levelGoalsProvider;
        private readonly IGameBoardDataProvider<TGridSlot> _gameBoardDataProvider;
        private readonly ISolvedSequencesConsumer<TGridSlot>[] _solvedSequencesConsumers;

        private bool _isStarted;
        private int _achievedGoals;

        private LevelGoal<TGridSlot>[] _levelGoals;
        private IBoardFillStrategy<TGridSlot> _fillStrategy;

        protected Match3Game(GameConfig<TGridSlot> config)
        {
            _gameBoard = new GameBoard<TGridSlot>(config.ItemSwapper, config.GameBoardSolver);
            _levelGoalsProvider = config.LevelGoalsProvider;
            _gameBoardDataProvider = config.GameBoardDataProvider;
            _solvedSequencesConsumers = config.SolvedSequencesConsumers;
        }

        protected AsyncLazy SwapItemsTask { get; private set; }
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

        public virtual async UniTask<bool> StartAsync()
        {
            if (_isStarted)
            {
                return false;
            }

            if (_fillStrategy == null)
            {
                throw new NullReferenceException(nameof(_fillStrategy));
            }

            await _gameBoard.FillAsync(_fillStrategy);

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.Achieved += OnLevelGoalAchieved;
            }

            _gameBoard.SequencesSolved += OnGameBoardSequencesSolved;
            _isStarted = true;

            return true;
        }

        public virtual bool Stop()
        {
            if (_isStarted == false)
            {
                return false;
            }

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.Achieved -= OnLevelGoalAchieved;
            }

            _gameBoard.SequencesSolved -= OnGameBoardSequencesSolved;
            _isStarted = false;

            return true;
        }

        public void SetGameBoardFillStrategy(IBoardFillStrategy<TGridSlot> fillStrategy)
        {
            _fillStrategy = fillStrategy;
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

        private void OnGameBoardSequencesSolved(object sender, IReadOnlyCollection<ItemSequence<TGridSlot>> sequences)
        {
            foreach (var sequencesConsumer in _solvedSequencesConsumers)
            {
                sequencesConsumer.OnSequencesSolved(sequences);
            }

            foreach (var levelGoal in _levelGoals)
            {
                if (levelGoal.IsAchieved == false)
                {
                    levelGoal.OnSequencesSolved(sequences);
                }
            }
        }

        private void OnLevelGoalAchieved(object sender, EventArgs e)
        {
            LevelGoalAchieved?.Invoke(this, (LevelGoal<TGridSlot>) sender);

            _achievedGoals++;
            if (_achievedGoals == _levelGoals.Length)
            {
                RaiseGameFinishedAsync().Forget();
            }
        }

        protected UniTask SwapItemsAsync(GridPosition position1, GridPosition position2)
        {
            if (SwapItemsTask?.Task.Status.IsCompleted() ?? true)
            {
                SwapItemsTask = _gameBoard.SwapItemsAsync(_fillStrategy, position1, position2).ToAsyncLazy();
            }

            return SwapItemsTask.Task;
        }

        private async UniTask RaiseGameFinishedAsync()
        {
            if (SwapItemsTask != null)
            {
                await SwapItemsTask;
            }

            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}