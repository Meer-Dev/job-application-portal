using System.Threading.Tasks;
using solvefy.task.Configuration.Dto;

namespace solvefy.task.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
