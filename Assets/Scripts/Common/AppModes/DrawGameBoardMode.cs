using System;
using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Structs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.AppModes
{
    public class DrawGameBoardMode : IAppMode, IDeactivatable
    {
        private readonly IInputSystem _inputSystem;
        private readonly IGameUiCanvas _gameUiCanvas;
        private readonly IGameBoardRenderer _gameBoardRenderer;

        private bool _isDrawMode;
        private bool _isInitialized;
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
            if (_isInitialized == false)
            {
                _isInitialized = true;
                _gameBoardRenderer.CreateGridTiles();
            }

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

        private void OnPointerDown(object sender, PointerEventArgs pointer)
        {
            if (pointer.Button == PointerEventData.InputButton.Left &&
                IsPointerOnGrid(pointer.WorldPosition, out _previousSlotPosition))
            {
                _isDrawMode = true;
                InvertGridTileState(_previousSlotPosition);
            }
        }

        private void OnPointerDrag(object sender, PointerEventArgs pointer)
        {
            if (_isDrawMode == false)
            {
                return;
            }

            if (IsPointerOnGrid(pointer.WorldPosition, out var slotPosition) == false)
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

        private bool IsPointerOnGrid(Vector3 worldPosition, out GridPosition gridSlotPosition)
        {
            return _gameBoardRenderer.IsPointerOnGrid(worldPosition, out gridSlotPosition);
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