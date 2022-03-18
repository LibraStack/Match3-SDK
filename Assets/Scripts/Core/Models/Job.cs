using Cysharp.Threading.Tasks;
using Match3.Core.Interfaces;

namespace Match3.Core.Models
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