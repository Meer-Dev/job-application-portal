using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace solvefy.task.Controllers
{
    public abstract class taskControllerBase: AbpController
    {
        protected taskControllerBase()
        {
            LocalizationSourceName = taskConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
