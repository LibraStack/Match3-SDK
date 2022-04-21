using System;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using Match3.App.Interfaces;
using Match3.Core;
using Match3.Core.Structs;
using UnityEngine;

namespace Common
{
    public class UnityGameBoardRenderer : MonoBehaviour, IUnityGameBoardRenderer, IGameBoardDataProvider<IUnityGridSlot>
    {
        [Space]
        [SerializeField] private int _rowCount = 9;
        [SerializeField] private int _columnCount = 9;

        [Space]
        [SerializeField] private float _tileSize = 0.6f;

        [Space]
        [SerializeField] private TileModel[] _gridTiles;

        private IGridTile[,] _gridSlotTiles;
        private IUnityGridSlot[,] _gameBoardSlots;

        private Vector3 _originPosition;
        private TileItemsPool _tileItemsPool;

        private void Awake()
        {
            _tileItemsPool = new TileItemsPool(_gridTiles, transform);
        }

        public IUnityGridSlot[,] GetGameBoardSlots(int level)
        {
            return _gameBoardSlots;
        }

        public void CreateGridTiles(int[,] data)
        {
            _gridSlotTiles = new IGridTile[_rowCount, _columnCount];
            _gameBoardSlots = new IUnityGridSlot[_rowCount, _columnCount];
            _originPosition = GetOriginPosition(_rowCount, _columnCount);

            CreateGridTiles(TileGroup.Available);
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

        public void SetNextGridTileGroup(GridPosition gridPosition)
        {
            var tileGroup = GetTileGroup(gridPosition);
            SetTile(gridPosition.RowIndex, gridPosition.ColumnIndex, GetNextAvailableGroup(tileGroup));
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

        public TileGroup GetTileGroup(GridPosition gridPosition)
        {
            return (TileGroup) _gridSlotTiles[gridPosition.RowIndex, gridPosition.ColumnIndex].GroupId;
        }

        public void ResetGridTiles()
        {
            SetTilesGroup(TileGroup.Available);
        }

        public void Dispose()
        {
            DisposeGridTiles();
            DisposeGameBoardData();
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

        private void CreateGridTiles(TileGroup defaultTileGroup)
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    var gridTile = GetTile(rowIndex, columnIndex, defaultTileGroup);

                    _gridSlotTiles[rowIndex, columnIndex] = gridTile;
                    _gameBoardSlots[rowIndex, columnIndex] =
                        new UnityGridSlot(gridTile, new GridPosition(rowIndex, columnIndex));
                }
            }
        }

        private void SetTilesGroup(TileGroup group)
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    SetTile(rowIndex, columnIndex, group);
                }
            }
        }

        private void SetTile(int rowIndex, int columnIndex, TileGroup group)
        {
            var currentTile = _gridSlotTiles[rowIndex, columnIndex];
            if (currentTile != null)
            {
                _tileItemsPool.ReturnGridTile(currentTile);
            }

            var gridTile = GetTile(rowIndex, columnIndex, group);

            _gridSlotTiles[rowIndex, columnIndex] = gridTile;
            _gameBoardSlots[rowIndex, columnIndex].SetState(gridTile);
        }

        private IGridTile GetTile(int rowIndex, int columnIndex, TileGroup group)
        {
            var gridTile = _tileItemsPool.GetGridTile(group);
            gridTile.SetWorldPosition(GetWorldPosition(rowIndex, columnIndex));

            return gridTile;
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

        private void DisposeGridTiles()
        {
            if (_gridSlotTiles == null)
            {
                return;
            }

            foreach (var gridSlotTile in _gridSlotTiles)
            {
                gridSlotTile.Dispose();
            }

            Array.Clear(_gridSlotTiles, 0, _gridSlotTiles.Length);
            _gridSlotTiles = null;
        }

        private void DisposeGameBoardData()
        {
            if (_gameBoardSlots == null)
            {
                return;
            }

            Array.Clear(_gameBoardSlots, 0, _gameBoardSlots.Length);
            _gameBoardSlots = null;
        }
    }
}