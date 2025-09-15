using System.Linq;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using solvefy.task.Authorization;
using solvefy.task.Candidate.Dto;

namespace solvefy.task.Candidate
{
    [AbpAuthorize(PermissionNames.Pages_Candidates)]
    public class CandidateAppService : AsyncCrudAppService<Entities.Candidate, CandidateDto, int, PagedAndSortedResultRequestDto, CreateCandidateDto, UpdateCandidateDto>, ICandidateAppService
    {
        public CandidateAppService(IRepository<Entities.Candidate, int> repository)
            : base(repository)
        {
        }

        protected override IQueryable<Entities.Candidate> CreateFilteredQuery(PagedAndSortedResultRequestDto input)
        {
            return Repository.GetAll().Include(x => x.JobPosition);
        }

        protected override CandidateDto MapToEntityDto(Entities.Candidate entity)
        {
            var dto = ObjectMapper.Map<CandidateDto>(entity);
            dto.JobPositionTitle = entity.JobPosition?.Title;
            return dto;
        }
    }
}