using Abp.Authorization;
using solvefy.task.Authorization.Roles;
using solvefy.task.Authorization.Users;

namespace solvefy.task.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
