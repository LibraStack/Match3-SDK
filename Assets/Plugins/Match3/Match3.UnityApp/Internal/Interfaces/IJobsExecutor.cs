using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Match3.UnityApp.Interfaces;

namespace Match3.UnityApp.Internal.Interfaces
{
    internal interface IJobsExecutor
    {
        UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs);
    }
}