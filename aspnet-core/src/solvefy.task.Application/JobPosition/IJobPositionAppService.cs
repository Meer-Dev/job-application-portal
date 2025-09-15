using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using solvefy.task.JobPosition.Dto;

namespace solvefy.task.JobPosition
{
    public interface IJobPositionAppService : IAsyncCrudAppService<JobPositionDto, int, PagedAndSortedResultRequestDto, CreateJobPositionDto, UpdateJobPositionDto>
    {
    }
}
