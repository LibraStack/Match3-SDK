using Cysharp.Threading.Tasks;
using Match3.App.Interfaces;

namespace Match3.App
{
    public abstract class Job : IJob
    {
        public int ExecutionOrder { get; }

        protected Job(int executionOrder)
        {
            ExecutionOrder = executionOrder;
        }

        public abstract UniTask ExecuteAsync();
    }
}