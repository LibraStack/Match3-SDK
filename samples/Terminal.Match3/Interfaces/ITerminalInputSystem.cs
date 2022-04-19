using System;
using System.Threading;

namespace Terminal.Match3.Interfaces
{
    public interface ITerminalInputSystem
    {
        event EventHandler<ConsoleKey> KeyPressed;
        event EventHandler Break;

        void StartMonitoring(CancellationToken cancellationToken = default);
        void StopMonitoring();
    }
}