using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Terminal.Match3.Interfaces;

namespace Terminal.Match3
{
    public class TerminalInputSystem : ITerminalInputSystem
    {
        private bool _isMonitoring;

        public event EventHandler<ConsoleKey> KeyPressed;
        public event EventHandler Break;

        public void StartMonitoring(CancellationToken cancellationToken = default)
        {
            if (_isMonitoring)
            {
                return;
            }

            _isMonitoring = true;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    do
                    {
                        ReadKey();
                    } while (_isMonitoring);
                }
                catch (OperationCanceledException exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void StopMonitoring()
        {
            _isMonitoring = false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ReadKey()
        {
            var keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Break?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                KeyPressed?.Invoke(this, keyInfo.Key);
            }
        }
    }
}