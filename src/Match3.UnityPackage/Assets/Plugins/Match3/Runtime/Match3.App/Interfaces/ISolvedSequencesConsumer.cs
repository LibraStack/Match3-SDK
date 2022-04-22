using Match3.Core.Interfaces;

namespace Match3.App.Interfaces
{
    public interface ISolvedSequencesConsumer<TGridSlot> where TGridSlot : IGridSlot
    {
        void OnSequencesSolved(SolvedData<TGridSlot> solvedData);
    }
}