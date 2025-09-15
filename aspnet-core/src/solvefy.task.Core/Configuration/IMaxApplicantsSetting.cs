using System.Threading.Tasks;

namespace solvefy.task.Configuration
{
    public interface IMaxApplicantsSetting
    {
        Task<int> GetMaxApplicantsPerPositionAsync();
    }
}