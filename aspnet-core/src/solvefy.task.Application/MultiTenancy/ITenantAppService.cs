using Abp.Application.Services;
using solvefy.task.MultiTenancy.Dto;

namespace solvefy.task.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

