using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using solvefy.task.Authorization;
using solvefy.task.JobPosition.Dto;

 namespace solvefy.task.JobPosition
{
    [AbpAuthorize(PermissionNames.Pages_JobPositions)]
    public class JobPositionAppService : AsyncCrudAppService<Entities.JobPosition, JobPositionDto, int, PagedAndSortedResultRequestDto, CreateJobPositionDto, UpdateJobPositionDto>, IJobPositionAppService
    {
        public JobPositionAppService(IRepository<Entities.JobPosition, int> repository)
            : base(repository)
        {
        }

        protected override IQueryable<Entities.JobPosition> CreateFilteredQuery(PagedAndSortedResultRequestDto input)
        {
            return Repository.GetAll();
        }
    }
}