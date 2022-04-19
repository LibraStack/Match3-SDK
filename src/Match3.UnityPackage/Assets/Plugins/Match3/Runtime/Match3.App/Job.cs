using System.Threading;
using Cysharp.Threading.Tasks;
using Match3.App.Interfaces;

namespace Match3.App
{
    public abstract class Job : IJob
    {
        protected Job(int executionOrder)
        {
            ExecutionOrder = executionOrder;
        }

        public int ExecutionOrder { get; }

        public abstract UniTask ExecuteAsync(CancellationToken cancellationToken = default);
    }
}