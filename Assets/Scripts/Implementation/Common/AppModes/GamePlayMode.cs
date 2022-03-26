using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Implementation.Common.Interfaces;
using Match3.Core;
using Match3.Core.Enums;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;
using UnityEngine;

namespace Implementation.Common.AppModes
{
    public class GamePlayMode : IAppMode, IDeactivatable, IDisposable
    {
        private readonly IAppContext _appContext;
        private readonly IInputSystem _inputSystem;
        private readonly IGameUiCanvas _gameUiCanvas;
        private readonly IGameScoreBoard _gameScoreBoard;
        private readonly IGameBoardRenderer _gameBoardRenderer;
        private readonly IBoardFillStrategy<IUnityItem>[] _boardFillStrategies;

        private bool _isDragMode;
        private int _achievedGoals;

        private ILevelGoal[] _levelGoals;
        private GridPosition _slotDownPosition;
        private IGameBoard<IUnityItem> _gameBoard;

        public event EventHandler Finished;

        public GamePlayMode(IAppContext appContext)
        {
            _appContext = appContext;
            _inputSystem = appContext.Resolve<IInputSystem>();
            _gameUiCanvas = appContext.Resolve<IGameUiCanvas>();
            _gameScoreBoard = appContext.Resolve<IGameScoreBoard>();
            _gameBoardRenderer = appContext.Resolve<IGameBoardRenderer>();
            _boardFillStrategies = appContext.Resolve<IBoardFillStrategy<IUnityItem>[]>();
        }

        public void Activate()
        {
            _gameBoard = CreateGameBoard(_appContext);
            _gameBoard.FillAsync(GetSelectedFillStrategy()).Forget();

            _levelGoals = _appContext.Resolve<ILevelGoalsProvider>().GetLevelGoals(_gameBoard);

            _gameBoard.SequencesSolved += OnGameBoardSequencesSolved;
            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.Achieved += OnLevelGoalAchieved;
            }
        }

        public void Deactivate()
        {
            _gameBoard.SequencesSolved -= OnGameBoardSequencesSolved;
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.Achieved -= OnLevelGoalAchieved;
            }
        }

        public void Dispose()
        {
            _gameBoard?.Dispose();
        }

        private IGameBoard<IUnityItem> CreateGameBoard(IAppContext appContext)
        {
            var data = appContext.Resolve<IGameBoardDataProvider>().GetGameBoardData();
            var itemSwapper = appContext.Resolve<IItemSwapper<IUnityItem>>();
            var gameBoardSolver = appContext.Resolve<IGameBoardSolver<IUnityItem>>();

            return new GameBoard<IUnityItem>(data, itemSwapper, gameBoardSolver);
        }

        private void OnPointerDown(object sender, Vector2 pointerWorldPosition)
        {
            if (IsPointerOnBoard(pointerWorldPosition, out _slotDownPosition) && IsOccupiedSlot(_slotDownPosition))
            {
                _isDragMode = true;
            }
        }

        private void OnPointerDrag(object sender, Vector2 pointerWorldPosition)
        {
            if (_isDragMode == false)
            {
                return;
            }

            if (IsPointerOnBoard(pointerWorldPosition, out var slotPosition) == false ||
                IsOccupiedSlot(slotPosition) == false)
            {
                _isDragMode = false;
                return;
            }

            if (IsSameSlot(slotPosition) || IsDiagonalSlot(slotPosition))
            {
                return;
            }

            _isDragMode = false;
            _gameBoard.SwapItemsAsync(GetSelectedFillStrategy(), _slotDownPosition, slotPosition).Forget();
        }

        private void OnGameBoardSequencesSolved(object sender, IReadOnlyCollection<ItemSequence<IUnityItem>> sequences)
        {
            _gameScoreBoard.RegisterSolvedSequences(sequences);

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.RegisterSolvedSequences(sequences);
            }
        }

        private void OnLevelGoalAchieved(object sender, EventArgs e)
        {
            _achievedGoals++;

            if (_achievedGoals == _levelGoals.Length)
            {
                Finished?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool IsPointerOnBoard(Vector2 pointerWorldPosition, out GridPosition slotDownPosition)
        {
            return _gameBoardRenderer.IsPointerOnBoard(pointerWorldPosition, out slotDownPosition);
        }

        private bool IsOccupiedSlot(GridPosition gridPosition)
        {
            return _gameBoard[gridPosition].State == GridSlotState.Occupied;
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

        private IBoardFillStrategy<IUnityItem> GetSelectedFillStrategy()
        {
            return _boardFillStrategies[_gameUiCanvas.SelectedFillStrategyIndex];
        }
    }
}