using System;
using System.Collections.Generic;
using Match3.App.Interfaces;
using Match3.App.Models;
using Match3.Core.Interfaces;

namespace Match3.App
{
    public abstract class LevelGoal<TItem> : ISolvedSequencesConsumer<TItem> where TItem : IItem
    {
        public bool IsAchieved { get; private set; }

        public event EventHandler Achieved;

        public abstract void RegisterSolvedSequences(IEnumerable<ItemSequence<TItem>> sequences);

        protected void MarkAchieved()
        {
            IsAchieved = true;
            Achieved?.Invoke(this, EventArgs.Empty);
        }
    }
}