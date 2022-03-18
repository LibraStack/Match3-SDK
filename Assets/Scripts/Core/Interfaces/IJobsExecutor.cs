using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Match3.Core.Interfaces
{
    public interface IJobsExecutor
    {
        UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs);
    }
}