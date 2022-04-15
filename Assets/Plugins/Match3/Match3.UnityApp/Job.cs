using Cysharp.Threading.Tasks;
using Match3.UnityApp.Interfaces;

namespace Match3.UnityApp
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