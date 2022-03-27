using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.Core.Models;
using Match3.Core.Structs;

namespace Match3.Core.Interfaces
{
    public interface IGameBoard<TItem> : IGrid, IDisposable where TItem : IItem
    {
        GridSlot<TItem> this[GridPosition gridPosition] { get; }
        GridSlot<TItem> this[int rowIndex, int columnIndex] { get; }

        event EventHandler<IReadOnlyCollection<ItemSequence<TItem>>> SequencesSolved;

        void Init(bool[,] gameBoardData);
        UniTask FillAsync(IBoardFillStrategy<TItem> fillStrategy);
        UniTask SwapItemsAsync(IBoardFillStrategy<TItem> fillStrategy, GridPosition position1, GridPosition position2);
    }
}