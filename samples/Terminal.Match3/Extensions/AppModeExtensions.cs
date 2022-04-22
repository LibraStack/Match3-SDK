using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Match3.Infrastructure.Extensions;
using Match3.Infrastructure.Interfaces;

namespace Terminal.Match3.Extensions
{
    public static class AppModeExtensions
    {
        public static TaskAwaiter GetAwaiter(this IGameMode gameMode)
        {
            var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            void OnFinished(object sender, EventArgs eventArgs)
            {
                gameMode.Finished -= OnFinished;
                gameMode.Deactivate();

                tcs.SetResult();
            }

            gameMode.Finished += OnFinished;
            gameMode.Activate();

            return tcs.Task.GetAwaiter();
        }
    }
}