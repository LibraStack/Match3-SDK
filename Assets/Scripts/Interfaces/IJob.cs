using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IJob
    {
        UniTask ExecuteAsync();
    }
}