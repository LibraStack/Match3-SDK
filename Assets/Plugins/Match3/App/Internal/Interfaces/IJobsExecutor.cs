using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.App.Interfaces;

namespace Match3.App.Internal.Interfaces
{
    internal interface IJobsExecutor
    {
        UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs);
    }
}