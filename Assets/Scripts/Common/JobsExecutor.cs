using System.Collections.Generic;
using Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace Common
{
    public class JobsExecutor : IJobsExecutor
    {
        public async UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs)
        {
            await UniTask.WhenAll(jobs.Select(job => job.ExecuteAsync()));
        }
    }
}