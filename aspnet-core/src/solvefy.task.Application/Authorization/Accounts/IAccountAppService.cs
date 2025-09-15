using System.Threading.Tasks;
using Abp.Application.Services;
using solvefy.task.Authorization.Accounts.Dto;

namespace solvefy.task.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
