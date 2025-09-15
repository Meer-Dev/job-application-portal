using Abp.Dependency;
using Abp.BackgroundJobs;

namespace solvefy.task.Jobs
{
    public class JobPositionCandidateCheckerJobScheduler : ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        public JobPositionCandidateCheckerJobScheduler(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
        }

        public void ScheduleJob()
        {
            // Simple job scheduling - will run once at startup
            _backgroundJobManager.Enqueue<JobPositionCandidateCheckerJob, object>(new object());
        }
    }
}