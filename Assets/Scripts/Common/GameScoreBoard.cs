using System.Collections.Generic;
using System.Text;
using Common.Interfaces;
using Match3.App.Interfaces;
using Match3.App.Models;
using UnityEngine;

namespace Common
{
    public class GameScoreBoard : IGameScoreBoard<IUnityItem>
    {
        public void RegisterSolvedSequences(IEnumerable<ItemSequence<IUnityItem>> sequences)
        {
            foreach (var sequence in sequences)
            {
                RegisterSequenceScore(sequence);
            }
        }

        private void RegisterSequenceScore(ItemSequence<IUnityItem> sequence)
        {
            Debug.Log(GetSequenceDescription(sequence));
        }

        private string GetSequenceDescription(ItemSequence<IUnityItem> sequence)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("ContentId <color=yellow>");
            stringBuilder.Append(sequence.SolvedGridSlots[0].Item.ContentId);
            stringBuilder.Append("</color> | <color=yellow>");
            stringBuilder.Append(sequence.SequenceDetectorType.Name);
            stringBuilder.Append("</color> sequence of <color=yellow>");
            stringBuilder.Append(sequence.SolvedGridSlots.Count);
            stringBuilder.Append("</color> elements");

            return stringBuilder.ToString();
        }
    }
}