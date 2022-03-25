using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Implementation.Common.Interfaces;
using Match3.Core;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;
using UnityEngine;

namespace Implementation.Common.AppModes
{
    public class GamePlayMode : IAppMode, IDeactivatable, IDisposable
    {
        private readonly IAppContext _appContext;
        private readonly IGameUiCanvas _gameUiCanvas;
        private readonly IInputSystem _inputSystem;
        private readonly IGameBoardRenderer _gameBoardRenderer;
        private readonly IGameScoreBoard<IUnityItem> _gameScoreBoard;
        private readonly IBoardFillStrategy<IUnityItem>[] _boardFillStrategies;

        private bool _isDragMode;

        private GridPosition _slotDownPosition;
        private IGameBoard<IUnityItem> _gameBoard;

        public event EventHandler Finished;

        public GamePlayMode(IAppContext appContext)
        {
            _appContext = appContext;
            _inputSystem = appContext.Resolve<IInputSystem>();
            _gameUiCanvas = appContext.Resolve<IGameUiCanvas>();
            _gameScoreBoard = appContext.Resolve<IGameScoreBoard<IUnityItem>>();
            _gameBoardRenderer = appContext.Resolve<IGameBoardRenderer>();
            _boardFillStrategies = appContext.Resolve<IBoardFillStrategy<IUnityItem>[]>();
        }

        public void Activate()
        {
            _gameBoard = CreateGameBoard(_appContext);
            _gameBoard.FillAsync(GetSelectedFillStrategy()).Forget();

            _gameBoard.SequencesSolved += OnGameBoardSequencesSolved;
            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;
        }

        public void Deactivate()
        {
            _gameBoard.SequencesSolved -= OnGameBoardSequencesSolved;
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;
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
            if (_gameBoard.IsFilled &&
                _gameBoardRenderer.IsPointerOnBoard(pointerWorldPosition, out _slotDownPosition))
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

            if (_gameBoardRenderer.IsPointerOnBoard(pointerWorldPosition, out var slotPosition) == false)
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

        private async UniTask OnGameBoardSequencesSolved(object sender, IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            await _gameScoreBoard.RegisterGameScoreAsync(sequences);
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