using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Common.Interfaces;
using FillStrategies.Jobs;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Enums;
using Match3.Core.Models;
using Match3.Core.Structs;
using UnityEngine;

namespace FillStrategies
{
    public abstract class BaseFillStrategy : IBoardFillStrategy<IUnityItem>
    {
        private readonly IItemsPool<IUnityItem> _itemsPool;
        private readonly IGameBoardAgreements _gameBoardAgreements;
        private readonly IUnityGameBoardRenderer _gameBoardRenderer;

        protected BaseFillStrategy(IAppContext appContext)
        {
            _itemsPool = appContext.Resolve<IItemsPool<IUnityItem>>();
            _gameBoardAgreements = appContext.Resolve<IGameBoardAgreements>();
            _gameBoardRenderer = appContext.Resolve<IUnityGameBoardRenderer>();
        }

        public abstract string Name { get; }

        public virtual IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityItem> gameBoard)
        {
            var itemsToShow = new List<IUnityItem>();

            for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
                {
                    var gridSlot = gameBoard[rowIndex, columnIndex];
                    if (CanSetItem(gridSlot) == false)
                    {
                        continue;
                    }

                    var item = GetItemFromPool();
                    item.SetWorldPosition(GetWorldPosition(gridSlot.GridPosition));

                    gridSlot.SetItem(item);
                    itemsToShow.Add(item);
                }
            }

            return new[] { new ItemsShowJob(itemsToShow) };
        }

        public abstract IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityItem> gameBoard,
            IEnumerable<ItemSequence<IUnityItem>> sequences);

        protected bool CanSetItem(GridSlot<IUnityItem> gridSlot)
        {
            var tileGroup = _gameBoardRenderer.GetTileGroup(gridSlot.GridPosition);
            return _gameBoardAgreements.CanSetItem(gridSlot, tileGroup);
        }

        protected bool IsMovableSlot(GridSlot<IUnityItem> gridSlot)
        {
            if (_gameBoardRenderer.IsLockedSlot(gridSlot.GridPosition))
            {
                return false;
            }

            return gridSlot.State == GridSlotState.Occupied;
        }

        protected bool IsAvailableSlot(GridSlot<IUnityItem> gridSlot)
        {
            if (_gameBoardRenderer.IsLockedSlot(gridSlot.GridPosition))
            {
                return false;
            }

            return gridSlot.State != GridSlotState.NotAvailable;
        }

        protected Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return _gameBoardRenderer.GetWorldPosition(gridPosition);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IUnityItem GetItemFromPool()
        {
            return _itemsPool.GetItem();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void ReturnItemToPool(IUnityItem item)
        {
            _itemsPool.ReturnItem(item);
        }
        
        protected bool CanMoveInDirection(IGameBoard<IUnityItem> gameBoard, GridSlot<IUnityItem> gridSlot,
            GridPosition direction, out GridPosition gridPosition)
        {
            var bottomGridSlot = GetSideGridSlot(gameBoard, gridSlot, direction);
            if (bottomGridSlot == null || CanSetItem(bottomGridSlot) == false)
            {
                gridPosition = GridPosition.Zero;
                return false;
            }

            gridPosition = bottomGridSlot.GridPosition;
            return true;
        }

        protected GridSlot<IUnityItem> GetSideGridSlot(IGameBoard<IUnityItem> gameBoard, GridSlot<IUnityItem> gridSlot,
            GridPosition direction)
        {
            var sideGridSlotPosition = gridSlot.GridPosition + direction;

            return gameBoard.IsPositionOnGrid(sideGridSlotPosition)
                ? gameBoard[sideGridSlotPosition]
                : null;
        }
    }
}