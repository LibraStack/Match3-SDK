using System.Collections.Generic;
using Match3.App;
using Match3.App.Models;
using Match3.Core.Interfaces;

namespace Match3.Tests.PlayMode.Mocks
{
    public class MockLevelGoal : LevelGoal<IItem>
    {
        public override void OnSequencesSolved(IEnumerable<ItemSequence<IItem>> sequences)
        {
            MarkAchieved();
        }
    }
}