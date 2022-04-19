using System;
using System.Collections.Generic;
using Match3.App.Interfaces;
using Match3.Core.Interfaces;

namespace Match3.App
{
    public abstract class LevelGoal<TGridSlot> : ISolvedSequencesConsumer<TGridSlot> where TGridSlot : IGridSlot
    {
        public bool IsAchieved { get; private set; }

        public event EventHandler Achieved;

        public abstract void OnSequencesSolved(IEnumerable<ItemSequence<TGridSlot>> sequences);

        protected void MarkAchieved()
        {
            IsAchieved = true;
            Achieved?.Invoke(this, EventArgs.Empty);
        }
    }
}