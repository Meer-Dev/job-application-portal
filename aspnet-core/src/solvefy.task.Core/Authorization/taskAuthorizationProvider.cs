using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace solvefy.task.Authorization
{
    public class TaskAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 👇 DO NOT REGISTER THESE — THEY ARE AUTO-REGISTERED BY ABP ZERO CORE!
            // context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            // context.CreatePermission(PermissionNames.Pages_Users_Activation, L("UsersActivation"));
            // context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            // context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            // ✅ ONLY ADD YOUR CUSTOM PERMISSIONS HERE
            //context.CreatePermission(PermissionNames.Pages_JobPositions, L("JobPositions"));
            //context.CreatePermission(PermissionNames.Pages_Candidates, L("Candidates"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, taskConsts.LocalizationSourceName);
        }
    }
}