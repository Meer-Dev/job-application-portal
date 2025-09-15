using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using solvefy.task.Candidate.Dto;

namespace solvefy.task.Candidate
{
    public interface ICandidateAppService : IAsyncCrudAppService<CandidateDto, int, PagedAndSortedResultRequestDto, CreateCandidateDto, UpdateCandidateDto>
    {
    }
}