using System;
using Interfaces;

namespace AppModes
{
    public abstract class AppMode<T> : IAppMode
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