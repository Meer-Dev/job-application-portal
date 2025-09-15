using System.Threading.Tasks;
using Abp.Application.Services;
using solvefy.task.Sessions.Dto;

namespace solvefy.task.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
