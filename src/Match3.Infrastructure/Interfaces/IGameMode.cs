using System;

namespace Match3.Infrastructure.Interfaces
{
    public interface IGameMode
    {
        event EventHandler Finished;

        void Activate();
    }
}