using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;

namespace solvefy.task.Configuration
{
    public class MaxApplicantsSetting : IMaxApplicantsSetting, ITransientDependency
    {
        private readonly ISettingManager _settingManager;

        public MaxApplicantsSetting(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task<int> GetMaxApplicantsPerPositionAsync()
        {
            var value = await _settingManager.GetSettingValueAsync(AppSettingNames.MaxApplicantsPerPosition);
            return int.Parse(value);
        }
    }
}