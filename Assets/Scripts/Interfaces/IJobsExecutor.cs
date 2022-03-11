using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Interfaces
{
    public interface IJobsExecutor
    {
        UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs);
    }
}