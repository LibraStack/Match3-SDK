using System;
using Interfaces;

namespace AppStates
{
    public abstract class AppState<T> : IAppState
    {
        public event EventHandler<T> Finished;

        public abstract void Activate();
        public abstract void Deactivate();

        protected void RaiseFinished(T eventArgs)
        {
            Finished?.Invoke(this, eventArgs);
        }
    }
}