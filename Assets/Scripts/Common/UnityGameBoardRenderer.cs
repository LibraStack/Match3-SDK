using System;
using System.Collections.Generic;
using Common.Enums;
using Common.GridSlotStates;
using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.Core.Helpers;
using Match3.Core.Interfaces;
using Match3.Core.Structs;
using UnityEngine;

namespace Common
{
    public class UnityGameBoardRenderer : MonoBehaviour, IUnityGameBoardRenderer, IGameBoardDataProvider
    {
        [Space]
        [SerializeField] private int _rowCount = 9;
        [SerializeField] private int _columnCount = 9;

        [Space]
        [SerializeField] private float _tileSize = 0.6f;
        [SerializeField] private TileItemsPool _tileItemsPool;

        private Vector3 _originPosition;
        private IGridTile[,] _gridSlotTiles;
        private IGridSlotState[,] _gameBoardData;
        private Dictionary<GridPosition, IStatefulSlot> _statefulSlots;

        public IGridSlotState[,] GetGameBoardData(int level)
        {
            if (_gameBoardData != null)
            {
                return _gameBoardData;
            }

            _gameBoardData = new IGridSlotState[_rowCount, _columnCount];
            _statefulSlots = new Dictionary<GridPosition, IStatefulSlot>();

            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    _gameBoardData[rowIndex, columnIndex] = GetGridSlotState(rowIndex, columnIndex);
                }
            }

            return _gameBoardData;
        }

        public void CreateGridTiles()
        {
            _gridSlotTiles = new IGridTile[_rowCount, _columnCount];
            _originPosition = GetOriginPosition(_rowCount, _columnCount);

            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    SetTile(rowIndex, columnIndex, TileGroup.Available);
                }
            }
        }

        public bool IsTileActive(GridPosition gridPosition)
        {
            return GetTileGroup(gridPosition) != TileGroup.Unavailable;
        }

        public void ActivateTile(GridPosition gridPosition)
        {
            SetTile(gridPosition.RowIndex, gridPosition.ColumnIndex, TileGroup.Available);
        }

        public void DeactivateTile(GridPosition gridPosition)
        {
            SetTile(gridPosition.RowIndex, gridPosition.ColumnIndex, TileGroup.Unavailable);
        }

        public bool IsPointerOnGrid(Vector3 worldPointerPosition, out GridPosition gridPosition)
        {
            gridPosition = GetGridPositionByPointer(worldPointerPosition);
            return IsPositionOnGrid(gridPosition);
        }

        public bool IsPointerOnBoard(Vector3 worldPointerPosition, out GridPosition gridPosition)
        {
            gridPosition = GetGridPositionByPointer(worldPointerPosition);
            return IsPositionOnBoard(gridPosition);
        }

        public bool IsPositionOnGrid(GridPosition gridPosition)
        {
            return GridMath.IsPositionOnGrid(gridPosition, _rowCount, _columnCount);
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return GetWorldPosition(gridPosition.RowIndex, gridPosition.ColumnIndex);
        }

        public void SetNextGridTileGroup(GridPosition gridPosition)
        {
            var tileGroup = GetTileGroup(gridPosition);
            SetTile(gridPosition.RowIndex, gridPosition.ColumnIndex, GetNextAvailableGroup(tileGroup));
        }

        public void TrySetNextTileState(GridPosition gridPosition)
        {
            if (_statefulSlots.TryGetValue(gridPosition, out var statefulSlot) == false)
            {
                return;
            }

            statefulSlot.NextState(); // TODO: Reset logic.
            SetNextTileState(gridPosition,
                (IStatefulSlot) _gridSlotTiles[gridPosition.RowIndex, gridPosition.ColumnIndex]);
        }

        public TileGroup GetTileGroup(GridPosition gridPosition)
        {
            return _gridSlotTiles[gridPosition.RowIndex, gridPosition.ColumnIndex].Group;
        }

        public void ResetState()
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    ResetGridSlotTile(rowIndex, columnIndex);
                }
            }

            ResetGameBoardData();
        }

        public void Dispose()
        {
            foreach (var gridSlotTile in _gridSlotTiles)
            {
                gridSlotTile.Dispose();
            }

            Array.Clear(_gridSlotTiles, 0, _gridSlotTiles.Length);
            _gridSlotTiles = null;

            ResetGameBoardData();
        }

        private bool IsPositionOnBoard(GridPosition gridPosition)
        {
            return IsPositionOnGrid(gridPosition) && IsTileActive(gridPosition);
        }

        private GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition)
        {
            var rowIndex = (worldPointerPosition - _originPosition).y / _tileSize;
            var columnIndex = (worldPointerPosition - _originPosition).x / _tileSize;

            return new GridPosition(Convert.ToInt32(-rowIndex), Convert.ToInt32(columnIndex));
        }

        private Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, -rowIndex) * _tileSize + _originPosition;
        }

        private Vector3 GetOriginPosition(int rowCount, int columnCount)
        {
            var offsetY = Mathf.Floor(rowCount / 2.0f) * _tileSize;
            var offsetX = Mathf.Floor(columnCount / 2.0f) * _tileSize;

            return new Vector3(-offsetX, offsetY);
        }

        private IGridTile GetTile(int rowIndex, int columnIndex, TileGroup group)
        {
            var tile = _tileItemsPool.GetTile(group);
            tile.SetWorldPosition(GetWorldPosition(rowIndex, columnIndex));

            return tile;
        }

        private void SetTile(int rowIndex, int columnIndex, TileGroup group)
        {
            var currentTile = _gridSlotTiles[rowIndex, columnIndex];
            if (currentTile != null)
            {
                _tileItemsPool.ReturnTile(currentTile);
            }

            _gridSlotTiles[rowIndex, columnIndex] = GetTile(rowIndex, columnIndex, group);
        }

        private void SetNextTileState(GridPosition gridPosition, IStatefulSlot statefulSlot)
        {
            var hasNextState = statefulSlot.NextState();
            if (hasNextState)
            {
                return;
            }

            SetTile(gridPosition.RowIndex, gridPosition.ColumnIndex, TileGroup.Available);
            statefulSlot.ResetState();
        }

        private TileGroup GetNextAvailableGroup(TileGroup group)
        {
            var index = (int) group + 1;
            var resultGroup = TileGroup.Available;
            var groupValues = (TileGroup[]) Enum.GetValues(typeof(TileGroup));

            if (index < groupValues.Length)
            {
                resultGroup = groupValues[index];
            }

            return resultGroup;
        }

        private IGridSlotState GetGridSlotState(int rowIndex, int columnIndex)
        {
            var group = _gridSlotTiles[rowIndex, columnIndex].Group;

            IGridSlotState gridSlotState = group switch
            {
                TileGroup.Unavailable => new NotAvailableState(),
                TileGroup.Available => new AvailableState(),
                TileGroup.Ice => new IceState(),
                TileGroup.Stone => new StoneState(),
                _ => throw new ArgumentOutOfRangeException()
            };

            if (gridSlotState is IStatefulSlot statefulSlot)
            {
                _statefulSlots.Add(new GridPosition(rowIndex, columnIndex), statefulSlot);
            }

            return gridSlotState;
        }

        private void ResetGridSlotTile(int rowIndex, int columnIndex)
        {
            SetTile(rowIndex, columnIndex, TileGroup.Available);
        }

        private void ResetGameBoardData()
        {
            if (_gameBoardData == null)
            {
                return;
            }

            Array.Clear(_gameBoardData, 0, _gameBoardData.Length);

            _statefulSlots?.Clear();
            _statefulSlots = null;
            _gameBoardData = null;
        }
    }
}