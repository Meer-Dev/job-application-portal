using System;
using System.Linq;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Logging;

namespace solvefy.task.Jobs
{
    public class JobPositionCandidateCheckerJob : BackgroundJob<object>, ITransientDependency
    {
        private readonly IRepository<Entities.JobPosition, int> _jobPositionRepository;
        private readonly IRepository<Entities.Candidate, int> _candidateRepository;

        public JobPositionCandidateCheckerJob(
            IRepository<Entities.JobPosition, int> jobPositionRepository,
            IRepository<Entities.Candidate, int> candidateRepository)
        {
            _jobPositionRepository = jobPositionRepository;
            _candidateRepository = candidateRepository;
        }

        public override void Execute(object args)
        {
            var sevenDaysAgo = DateTime.Now.AddDays(-7);
            var activeJobPositions = _jobPositionRepository.GetAll()
                .Where(jp => jp.IsActive)
                .ToList();

            foreach (var jobPosition in activeJobPositions)
            {
                var candidateCount = _candidateRepository.Count(c =>
                    c.JobPositionId == jobPosition.Id &&
                    c.CreationTime >= sevenDaysAgo);

                if (candidateCount < 3)
                {
                    LogHelper.Logger.Warn($"JobPosition '{jobPosition.Title}' (ID: {jobPosition.Id}) has only {candidateCount} candidates in the last 7 days");
                }
            }
        }
    }
}