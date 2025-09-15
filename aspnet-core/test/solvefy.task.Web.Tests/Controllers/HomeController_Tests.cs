using System.Threading.Tasks;
using solvefy.task.Models.TokenAuth;
using solvefy.task.Web.Controllers;
using Shouldly;
using Xunit;

namespace solvefy.task.Web.Tests.Controllers
{
    public class HomeController_Tests: taskWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}