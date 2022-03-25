using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.Core.Delegates;
using Match3.Core.Enums;
using Match3.Core.Helpers;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Match3.Core
{
    public class GameBoard<TItem> : IGameBoard<TItem> where TItem : IItem
    {
        private readonly int _rowCount;
        private readonly int _columnCount;

        private readonly GridSlot<TItem>[,] _gridSlots;

        private readonly IJobsExecutor _jobsExecutor;
        private readonly IItemSwapper<TItem> _itemSwapper;
        private readonly IGameBoardSolver<TItem> _gameBoardSolver;

        public bool IsFilled { get; private set; }

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;

        public GridSlot<TItem> this[GridPosition gridPosition] => _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex];
        public GridSlot<TItem> this[int rowIndex, int columnIndex] => _gridSlots[rowIndex, columnIndex];

        public event AsyncEventHandler<IEnumerable<ItemSequence<TItem>>> SequencesSolved;

        public GameBoard(bool[,] gameBoardData, IItemSwapper<TItem> itemSwapper, IGameBoardSolver<TItem> gameBoardSolver)
        {
            _itemSwapper = itemSwapper;
            _jobsExecutor = new JobsExecutor();
            _gameBoardSolver = gameBoardSolver;

            _rowCount = gameBoardData.GetLength(0);
            _columnCount = gameBoardData.GetLength(1);

            _gridSlots = new GridSlot<TItem>[_rowCount, _columnCount];

            CreateGridSlots(gameBoardData);
        }

        public async UniTask FillAsync(IBoardFillStrategy<TItem> fillStrategy)
        {
            await _jobsExecutor.ExecuteJobsAsync(fillStrategy.GetFillJobs(this));

            IsFilled = true;
        }

        public async UniTask SwapItemsAsync(IBoardFillStrategy<TItem> fillStrategy, GridPosition position1,
            GridPosition position2)
        {
            await SwapItems(position1, position2);

            if (IsSolved(position1, position2, out var sequences))
            {
                var solveTask = _jobsExecutor.ExecuteJobsAsync(fillStrategy.GetSolveJobs(this, sequences));
                var raiseTask = RaiseSequencesSolvedAsync(sequences);

                await solveTask;
                await raiseTask;
            }
            else
            {
                await SwapItems(position1, position2);
            }
        }

        public bool IsPositionOnGrid(GridPosition gridPosition)
        {
            return GridMath.IsPositionOnGrid(this, gridPosition);
        }

        public bool IsPositionOnBoard(GridPosition gridPosition)
        {
            if (IsPositionOnGrid(gridPosition) == false)
            {
                return false;
            }

            return _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex].State != GridSlotState.NotAvailable;
        }

        public void Dispose()
        {
            Array.Clear(_gridSlots, 0, _gridSlots.Length);
        }

        private async UniTask SwapItems(GridPosition position1, GridPosition position2)
        {
            var item1 = _gridSlots[position1.RowIndex, position1.ColumnIndex].Item;
            var item2 = _gridSlots[position2.RowIndex, position2.ColumnIndex].Item;

            await _itemSwapper.SwapItemsAsync(item1, item2);

            _gridSlots[position1.RowIndex, position1.ColumnIndex].SetItem(item2);
            _gridSlots[position2.RowIndex, position2.ColumnIndex].SetItem(item1);
        }

        private bool IsSolved(GridPosition position1, GridPosition position2,
            out IReadOnlyCollection<ItemSequence<TItem>> sequences)
        {
            sequences = _gameBoardSolver.Solve(this, position1, position2);
            return sequences.Count > 0;
        }

        private void CreateGridSlots(bool[,] gameBoardData)
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    var isTileActive = gameBoardData[rowIndex, columnIndex];

                    _gridSlots[rowIndex, columnIndex] = new GridSlot<TItem>(
                        isTileActive ? GridSlotState.Free : GridSlotState.NotAvailable,
                        new GridPosition(rowIndex, columnIndex));
                }
            }
        }

        private async UniTask RaiseSequencesSolvedAsync(IEnumerable<ItemSequence<TItem>> sequences)
        {
            if (SequencesSolved != null)
            {
                await SequencesSolved(this, sequences);
            }
        }
    }
}