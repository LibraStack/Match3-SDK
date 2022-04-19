using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Match3.App.Interfaces;

namespace Match3.App.Internal
{
    internal class JobsExecutor
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs, CancellationToken cancellationToken = default)
        {
            var jobGroups = jobs.GroupBy(job => job.ExecutionOrder).OrderBy(group => group.Key);

            foreach (var jobGroup in jobGroups)
            {
                await UniTask.WhenAll(jobGroup.Select(job => job.ExecuteAsync(cancellationToken)));
            }
        }
    }
}