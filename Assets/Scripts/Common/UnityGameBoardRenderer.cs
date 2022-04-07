using System;
using Common.Enums;
using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.Core.Helpers;
using Match3.Core.Structs;
using UnityEngine;

namespace Common
{
    public class UnityGameBoardRenderer : MonoBehaviour, IUnityGameBoardRenderer, IGameBoardDataProvider
    {
        [SerializeField] private AppContext _appContext;

        [Space]
        [SerializeField] private int _rowCount = 9;
        [SerializeField] private int _columnCount = 9;

        [Space]
        [SerializeField] private float _tileSize = 0.6f;
        [SerializeField] private TileItemsPool _tileItemsPool;

        private bool[,] _gameBoardData;

        private Vector3 _originPosition;
        private IGridTile[,] _gridSlotTiles;
        private IGameBoardAgreements _gameBoardAgreements;

        private void Start()
        {
            _gameBoardAgreements = _appContext.Resolve<IGameBoardAgreements>();
        }

        public bool[,] GetGameBoardData(int level)
        {
            return _gameBoardData;
        }

        public void CreateGridTiles()
        {
            _gameBoardData = new bool[_rowCount, _columnCount];
            _gridSlotTiles = new IGridTile[_rowCount, _columnCount];
            _originPosition = GetOriginPosition(_rowCount, _columnCount);

            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    _gameBoardData[rowIndex, columnIndex] = true;
                    SetTile(rowIndex, columnIndex, TileGroup.Available);
                }
            }
        }

        public bool IsTileActive(GridPosition gridPosition)
        {
            return _gameBoardData[gridPosition.RowIndex, gridPosition.ColumnIndex];
        }

        public void ActivateTile(GridPosition gridPosition)
        {
            _gameBoardData[gridPosition.RowIndex, gridPosition.ColumnIndex] = true;
            SetTile(gridPosition.RowIndex, gridPosition.ColumnIndex, TileGroup.Available);
        }

        public void DeactivateTile(GridPosition gridPosition)
        {
            _gameBoardData[gridPosition.RowIndex, gridPosition.ColumnIndex] = false;
            SetTile(gridPosition.RowIndex, gridPosition.ColumnIndex, TileGroup.Unavailable);
        }

        public bool IsLockedSlot(GridPosition gridPosition)
        {
            return _gameBoardAgreements.IsLockedSlot(GetTileGroup(gridPosition));
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

        public bool TrySetNextTileState(GridPosition gridPosition)
        {
            var tile = _gridSlotTiles[gridPosition.RowIndex, gridPosition.ColumnIndex];
            if (tile is IStatefulTile statefulTile)
            {
                return SetNextTileState(gridPosition, statefulTile);
            }

            return false;
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
        }

        public void Dispose()
        {
            foreach (var gridSlotTile in _gridSlotTiles)
            {
                gridSlotTile.Dispose();
            }

            Array.Clear(_gridSlotTiles, 0, _gridSlotTiles.Length);
            Array.Clear(_gameBoardData, 0, _gameBoardData.Length);

            _gridSlotTiles = null;
            _gameBoardData = null;
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

        private void ResetGridSlotTile(int rowIndex, int columnIndex)
        {
            _gameBoardData[rowIndex, columnIndex] = true;
            SetTile(rowIndex, columnIndex, TileGroup.Available);
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

        private bool SetNextTileState(GridPosition gridPosition, IStatefulTile statefulTile)
        {
            var hasNextState = statefulTile.NextState();
            if (hasNextState)
            {
                return true;
            }

            SetTile(gridPosition.RowIndex, gridPosition.ColumnIndex, TileGroup.Available);
            statefulTile.ResetState();

            return false;
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
    }
}