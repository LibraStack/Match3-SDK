using System;

namespace Common.Interfaces
{
    public interface IAppMode
    {
        event EventHandler Finished;

        void Activate();
        void Deactivate();
    }
}