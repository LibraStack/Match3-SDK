using System;

namespace Match3.Template.Interfaces
{
    public interface IGameMode
    {
        event EventHandler Finished;

        void Activate();
    }
}