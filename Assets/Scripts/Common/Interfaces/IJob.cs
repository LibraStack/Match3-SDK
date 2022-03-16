using Cysharp.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IJob
    {
        int ExecutionOrder { get; }
        
        UniTask ExecuteAsync();
    }
}