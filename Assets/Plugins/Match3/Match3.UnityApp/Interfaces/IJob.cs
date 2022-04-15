using Cysharp.Threading.Tasks;

namespace Match3.UnityApp.Interfaces
{
    public interface IJob
    {
        int ExecutionOrder { get; }

        UniTask ExecuteAsync();
    }
}