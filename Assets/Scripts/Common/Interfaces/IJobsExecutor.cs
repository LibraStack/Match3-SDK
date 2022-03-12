using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IJobsExecutor
    {
        UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs);
    }
}