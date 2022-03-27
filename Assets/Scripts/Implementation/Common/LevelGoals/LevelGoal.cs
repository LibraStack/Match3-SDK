using System;
using System.Collections.Generic;
using Implementation.Common.Interfaces;
using Match3.Core.Models;

namespace Implementation.Common.LevelGoals
{
    public abstract class LevelGoal : ISolvedSequencesConsumer<IUnityItem>
    {
        public bool IsAchieved { get; private set; }

        public event EventHandler Achieved;

        public abstract void RegisterSolvedSequences(IEnumerable<ItemSequence<IUnityItem>> sequences);

        protected void MarkAchieved()
        {
            IsAchieved = true;
            Achieved?.Invoke(this, EventArgs.Empty);
        }
    }
}