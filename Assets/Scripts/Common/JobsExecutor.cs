using System.Collections.Generic;
using System.Linq;
using Common.Interfaces;
using Cysharp.Threading.Tasks;

namespace Common
{
    public class JobsExecutor : IJobsExecutor
    {
        public async UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs)
        {
            var jobGroups = jobs.GroupBy(job => job.Priority).OrderBy(group => group.Key);

            foreach (var jobGroup in jobGroups)
            {
                await UniTask.WhenAll(jobGroup.Select(job => job.ExecuteAsync()));
            }
        }
    }
}