using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Match3.App.Interfaces;
using Match3.App.Internal.Interfaces;

namespace Match3.App.Internal
{
    internal class JobsExecutor : IJobsExecutor
    {
        public async UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs)
        {
            var jobGroups = jobs.GroupBy(job => job.ExecutionOrder).OrderBy(group => group.Key);

            foreach (var jobGroup in jobGroups)
            {
                await UniTask.WhenAll(jobGroup.Select(job => job.ExecuteAsync()));
            }
        }
    }
}
