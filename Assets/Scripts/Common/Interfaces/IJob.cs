using Cysharp.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IJob
    {
        int Priority { get; }
        
        UniTask ExecuteAsync();
    }
}