using System.Collections.Generic;
using Abp.Configuration;
using Abp.Localization;

namespace solvefy.task.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(
                    AppSettingNames.MaxApplicantsPerPosition,
                    "50",
                    L("MaxApplicantsPerPosition"),
                    scopes: SettingScopes.Application | SettingScopes.Tenant,
                    isVisibleToClients: true
                )
            };
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, taskConsts.LocalizationSourceName);
        }
    }

    public static class AppSettingNames
    {
        public const string MaxApplicantsPerPosition = "App.Job.MaxApplicantsPerPosition";
        public static string UiTheme;
    }
}