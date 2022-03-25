using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Implementation.Common.Interfaces;
using Match3.Core.Interfaces;
using Match3.Core.Models;
using UnityEngine;

namespace Implementation.Common
{
    public class GameScoreBoard : IGameScoreBoard<IUnityItem>
    {
        public async UniTask RegisterGameScoreAsync(IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            foreach (var itemSequence in sequences)
            {
                Debug.Log(
                    $"<color=yellow>{itemSequence.Type}</color> sequence of <color=yellow>{itemSequence.SolvedGridSlots.Count}</color> elements");

                await UniTask.Yield();
            }
        }
    }
}