using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Controllers;
using Abp.Web.Configuration;

namespace solvefy.task.Web.Host.Controllers
{
    public class AbpUserConfigurationController : AbpController
    {
        private readonly AbpUserConfigurationBuilder _abpUserConfigurationBuilder;

        public AbpUserConfigurationController(AbpUserConfigurationBuilder abpUserConfigurationBuilder)
        {
            _abpUserConfigurationBuilder = abpUserConfigurationBuilder;
        }

        public JsonResult GetAll()
        {
            return Json(_abpUserConfigurationBuilder.GetAll());
        }
    }
}