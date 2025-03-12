using Moq;
using EindOpdracht.WebApi;
using EindOpdracht.WebApi.Services;
using Castle.Core.Logging;
using EindOpdracht.WebApi.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Wk2_Eindopracht_test
{
    [TestClass]
    public class StudentTests
    {
        private Mock<IEnvironment2DRepository> _mockEnvironmentRepo;
        private Mock<IAuthenticationService> _mockAuthenticationService;
        private Mock<ILogger<Environment2DController>> _mockLogger;
        private Environment2DController _environment2dController;

        //[TestMethod]
        //public void Student_WrittenFirstUnitTest_Succesfully()
        //{
        //    Assert.IsTrue(true);

        //    var userId = Guid.NewGuid().ToString();
        //}

        [TestInitialize]
        public void Setup()
        {
            _mockEnvironmentRepo = new Mock<IEnvironment2DRepository>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();
            _mockLogger = new Mock<ILogger<Environment2DController>>();
            _environment2dController = new Environment2DController(_mockEnvironmentRepo.Object, _mockLogger.Object, _mockAuthenticationService.Object);
        }

        [TestMethod]
        public async Task Add_CreatedAtRoute_IfEnvironment2DIsSuccesfullyCreated()
        {
            var userId = Guid.NewGuid().ToString();
            var environmentNew = new Environment2D { Name = "A new hope", OwnerUserId = userId};
            _mockAuthenticationService!.Setup(getUserId => getUserId.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockEnvironmentRepo!.Setup(newEnv => newEnv.InsertAsync(environmentNew)).ReturnsAsync(environmentNew);

            var result = await _environment2dController!.Add(environmentNew);
            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteResult));
        }
    }
}
