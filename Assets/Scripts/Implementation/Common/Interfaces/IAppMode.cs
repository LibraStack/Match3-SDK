using System;

namespace Implementation.Common.Interfaces
{
    public interface IAppMode
    {
        event EventHandler Finished;

        void Activate();
    }
}