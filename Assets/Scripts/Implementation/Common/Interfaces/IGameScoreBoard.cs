using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.Core.Interfaces;
using Match3.Core.Models;

namespace Implementation.Common.Interfaces
{
    public interface IGameScoreBoard<TItem> where TItem : IItem
    {
        UniTask RegisterGameScoreAsync(IEnumerable<ItemSequence<TItem>> sequences);
    }
}