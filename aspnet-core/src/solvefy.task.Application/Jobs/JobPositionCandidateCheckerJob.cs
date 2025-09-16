using System;
using System.Linq;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

// ✅ alias the entity classes explicitly
using JobPositionEntity = solvefy.task.Entities.JobPosition;
using CandidateEntity = solvefy.task.Entities.Candidate;

namespace solvefy.task.Jobs
{
    public class JobPositionCandidateCheckerJob : BackgroundJob<object>, ITransientDependency
    {
        private readonly IRepository<JobPositionEntity, int> _jobPositionRepository;
        private readonly IRepository<CandidateEntity, int> _candidateRepository;

        public JobPositionCandidateCheckerJob(
            IRepository<JobPositionEntity, int> jobPositionRepository,
            IRepository<CandidateEntity, int> candidateRepository)
        {
            _jobPositionRepository = jobPositionRepository;
            _candidateRepository = candidateRepository;
        }

        [UnitOfWork]
        public override void Execute(object args)
        {
            try
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
                        Logger.Warn(
                            $"JobPosition '{jobPosition.Title}' (ID: {jobPosition.Id}) " +
                            $"has only {candidateCount} candidates in the last 7 days"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error in JobPositionCandidateCheckerJob: " + ex.Message, ex);
                throw;
            }
        }
    }
}
