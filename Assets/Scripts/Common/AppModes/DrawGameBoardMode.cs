using System;
using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.Core.Structs;
using UnityEngine;

namespace Common.AppModes
{
    public class DrawGameBoardMode : IAppMode, IDeactivatable
    {
        private readonly IInputSystem _inputSystem;
        private readonly IGameUiCanvas _gameUiCanvas;
        private readonly IGameBoardRenderer _gameBoardRenderer;

        private bool _isDrawMode;
        private GridPosition _previousSlotPosition;

        public event EventHandler Finished;

        public DrawGameBoardMode(IAppContext appContext)
        {
            _inputSystem = appContext.Resolve<IInputSystem>();
            _gameUiCanvas = appContext.Resolve<IGameUiCanvas>();
            _gameBoardRenderer = appContext.Resolve<IGameBoardRenderer>();
        }

        public void Activate()
        {
            _gameBoardRenderer.CreateGridTiles();

            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;
            _gameUiCanvas.StartGameClick += OnStartGameClick;
        }

        public void Deactivate()
        {
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;
            _gameUiCanvas.StartGameClick -= OnStartGameClick;
        }

        private void OnPointerDown(object sender, Vector2 mouseWorldPosition)
        {
            if (_gameBoardRenderer.IsPointerOnGrid(mouseWorldPosition, out _previousSlotPosition))
            {
                _isDrawMode = true;
                InvertGridTileState(_previousSlotPosition);
            }
        }

        private void OnPointerDrag(object sender, Vector2 mouseWorldPosition)
        {
            if (_isDrawMode == false)
            {
                return;
            }

            if (_gameBoardRenderer.IsPointerOnGrid(mouseWorldPosition, out var slotPosition) == false)
            {
                return;
            }

            if (IsSameSlot(slotPosition))
            {
                return;
            }

            _previousSlotPosition = slotPosition;
            InvertGridTileState(slotPosition);
        }

        private void OnStartGameClick(object sender, EventArgs e)
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private bool IsSameSlot(GridPosition slotPosition)
        {
            return _previousSlotPosition.Equals(slotPosition);
        }

        private void InvertGridTileState(GridPosition slotPosition)
        {
            if (_gameBoardRenderer.IsTileActive(slotPosition))
            {
                _gameBoardRenderer.DeactivateTile(slotPosition);
            }
            else
            {
                _gameBoardRenderer.ActivateTile(slotPosition);
            }
        }
    }
}