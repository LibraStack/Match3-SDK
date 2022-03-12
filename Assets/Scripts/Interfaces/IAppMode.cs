using System;

namespace Interfaces
{
    public interface IAppMode
    {
        event EventHandler Finished;

        void Activate();
        void Deactivate();
    }
}