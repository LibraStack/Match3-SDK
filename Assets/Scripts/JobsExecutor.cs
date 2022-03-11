using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Interfaces;

public class JobsExecutor : IJobsExecutor
{
    public async UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs)
    {
        await UniTask.WhenAll(jobs.Select(job => job.ExecuteAsync()));
    }
}