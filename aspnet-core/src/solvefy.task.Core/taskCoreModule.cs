using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero;
using Abp.Zero.Configuration;
using solvefy.task.Authorization.Roles;
using solvefy.task.Authorization.Users;
using solvefy.task.Configuration;
using solvefy.task.MultiTenancy;
using Abp.BackgroundJobs;

namespace solvefy.task
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class taskCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            // Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            Configuration.Settings.Providers.Add<AppSettingProvider>();

            Configuration.Authorization.Providers.Add<Authorization.TaskAuthorizationProvider>();

            Configuration.MultiTenancy.IsEnabled = taskConsts.MultiTenancyEnabled;

            // Enable background jobs
            Configuration.BackgroundJobs.IsJobExecutionEnabled = true;

            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(taskCoreModule).GetAssembly());
        }
    }
}