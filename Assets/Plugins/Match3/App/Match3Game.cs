using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.App.Interfaces;
using Match3.App.Internal;
using Match3.App.Models;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;
using UnityEngine;

namespace Match3.App
{
    public class Match3Game<TItem> : IDisposable where TItem : IItem
    {
        private readonly IInputSystem _inputSystem;
        private readonly GameBoard<TItem> _gameBoard;
        private readonly IGameBoardRenderer _gameBoardRenderer;
        private readonly ILevelGoalsProvider<TItem> _levelGoalsProvider;
        private readonly IGameBoardDataProvider<TItem> _gameBoardDataProvider;
        private readonly ISolvedSequencesConsumer<TItem>[] _solvedSequencesConsumers;

        private bool _isStarted;
        private bool _isDragMode;
        private int _achievedGoals;

        private AsyncLazy _swapItemsTask;
        private GridPosition _slotDownPosition;
        private LevelGoal<TItem>[] _levelGoals;
        private IBoardFillStrategy<TItem> _fillStrategy;

        public event EventHandler Finished;
        public event EventHandler<LevelGoal<TItem>> LevelGoalAchieved;

        public Match3Game(GameConfig<TItem> config)
        {
            _gameBoard = new GameBoard<TItem>(config.ItemSwapper, config.GameBoardSolver);
            _inputSystem = config.InputSystem;
            _fillStrategy = config.FillStrategy;
            _gameBoardRenderer = config.GameBoardRenderer;
            _levelGoalsProvider = config.LevelGoalsProvider;
            _gameBoardDataProvider = config.GameBoardDataProvider;
            _solvedSequencesConsumers = config.SolvedSequencesConsumers;
        }

        public void InitGameLevel(int level)
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Can not be initialized while the current game is active.");
            }

            _gameBoard.SetGridSlots(_gameBoardDataProvider.GetGameBoardSlots(level));
            _levelGoals = _levelGoalsProvider.GetLevelGoals(level, _gameBoard);
        }

        public async UniTask StartAsync()
        {
            if (_isStarted)
            {
                return;
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
            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;

            _isStarted = true;
        }

        public void Stop()
        {
            if (_isStarted == false)
            {
                return;
            }

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.Achieved -= OnLevelGoalAchieved;
            }

            _gameBoard.SequencesSolved -= OnGameBoardSequencesSolved;
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;

            _isStarted = false;
        }

        public void SetGameBoardFillStrategy(IBoardFillStrategy<TItem> fillStrategy)
        {
            _fillStrategy = fillStrategy;
        }

        public IEnumerable<GridSlot<TItem>> GetGridSlots()
        {
            for (var rowIndex = 0; rowIndex < _gameBoard.RowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _gameBoard.ColumnCount; columnIndex++)
                {
                    yield return _gameBoard[rowIndex, columnIndex];
                }
            }
        }

        public void ResetGameBoard()
        {
            _achievedGoals = 0;
            _gameBoard.ResetState();
            _gameBoardRenderer.ResetState();
        }

        public void Dispose()
        {
            _gameBoard?.Dispose();
            _gameBoardRenderer?.Dispose();
        }

        private void OnGameBoardSequencesSolved(object sender, IReadOnlyCollection<ItemSequence<TItem>> sequences)
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
            LevelGoalAchieved?.Invoke(this, (LevelGoal<TItem>) sender);

            _achievedGoals++;
            if (_achievedGoals == _levelGoals.Length)
            {
                RaiseGameFinishedAsync().Forget();
            }
        }

        private void OnPointerDown(object sender, PointerEventArgs pointer)
        {
            if (IsPointerOnBoard(pointer.WorldPosition, out _slotDownPosition) && IsMovableSlot(_slotDownPosition))
            {
                _isDragMode = true;
            }
        }

        private void OnPointerDrag(object sender, PointerEventArgs pointer)
        {
            if (_isDragMode == false)
            {
                return;
            }

            if (IsPointerOnBoard(pointer.WorldPosition, out var slotPosition) == false ||
                IsMovableSlot(slotPosition) == false)
            {
                _isDragMode = false;
                return;
            }

            if (IsSameSlot(slotPosition) || IsDiagonalSlot(slotPosition))
            {
                return;
            }

            _isDragMode = false;
            SwapItemsAsync(_fillStrategy, _slotDownPosition, slotPosition).Forget();
        }

        private UniTask SwapItemsAsync(IBoardFillStrategy<TItem> fillStrategy, GridPosition position1,
            GridPosition position2)
        {
            if (_swapItemsTask?.Task.Status.IsCompleted() ?? true)
            {
                _swapItemsTask = _gameBoard.SwapItemsAsync(fillStrategy, position1, position2).ToAsyncLazy();
            }

            return _swapItemsTask.Task;
        }

        private async UniTask RaiseGameFinishedAsync()
        {
            if (_swapItemsTask != null)
            {
                await _swapItemsTask;
            }

            Finished?.Invoke(this, EventArgs.Empty);
        }

        private bool IsPointerOnBoard(Vector2 pointerWorldPosition, out GridPosition slotDownPosition)
        {
            return _gameBoardRenderer.IsPointerOnBoard(pointerWorldPosition, out slotDownPosition);
        }

        private bool IsMovableSlot(GridPosition gridPosition)
        {
            return _gameBoard[gridPosition].IsMovable;
        }

        private bool IsSameSlot(GridPosition slotPosition)
        {
            return _slotDownPosition.Equals(slotPosition);
        }

        private bool IsDiagonalSlot(GridPosition slotPosition)
        {
            var isSideSlot = slotPosition.Equals(_slotDownPosition + GridPosition.Up) ||
                             slotPosition.Equals(_slotDownPosition + GridPosition.Down) ||
                             slotPosition.Equals(_slotDownPosition + GridPosition.Left) ||
                             slotPosition.Equals(_slotDownPosition + GridPosition.Right);

            return isSideSlot == false;
        }
    }
}