using System.Threading;
using Cysharp.Threading.Tasks;

namespace Match3.App.Interfaces
{
    public interface IJob
    {
        int ExecutionOrder { get; }

        UniTask ExecuteAsync(CancellationToken cancellationToken = default);
    }
}