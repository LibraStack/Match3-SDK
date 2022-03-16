using Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace Common.Models
{
    public abstract class Job : IJob
    {
        public int Priority { get; }

        protected Job(int priority)
        {
            Priority = priority;
        }
        
        public abstract UniTask ExecuteAsync();
    }
}