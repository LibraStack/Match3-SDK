using System;

namespace Implementation.Common.Interfaces
{
    public interface ILevelGoal : ISolvedSequencesConsumer<IUnityItem>
    {
        event EventHandler Achieved;
    }
}