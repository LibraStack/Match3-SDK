using Cysharp.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IJob
    {
        UniTask ExecuteAsync();
    }
}