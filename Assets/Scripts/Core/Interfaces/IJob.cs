using Cysharp.Threading.Tasks;

namespace Match3.Core.Interfaces
{
    public interface IJob
    {
        int ExecutionOrder { get; }
        
        UniTask ExecuteAsync();
    }
}