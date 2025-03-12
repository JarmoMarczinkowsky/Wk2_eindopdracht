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
        private Mock<IObject2dRepository> _mockObject2dRepo;
        private Mock<IObject2dRepository> _mockObjectRepo;
        private Mock<IAuthenticationService> _mockAuthenticationService;
        private Mock<ILogger<Environment2DController>> _mockLogger;
        private Mock<ILogger<Object2DController>> _mockObject2dLogger;
        private Environment2DController _environment2dController;
        private Object2DController _object2dController;

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
            _mockObject2dRepo = new Mock<IObject2dRepository>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();

            _mockLogger = new Mock<ILogger<Environment2DController>>();
            _mockObject2dLogger = new Mock<ILogger<Object2DController>>();
            
            _environment2dController = new Environment2DController(_mockEnvironmentRepo.Object, _mockLogger.Object, _mockAuthenticationService.Object);
            _object2dController = new Object2DController(_mockObject2dRepo.Object, _mockObject2dLogger.Object, _mockAuthenticationService.Object);
        }

        [TestMethod]
        public async Task Add_IfEnvironment2DIsSuccesfullyCreated()
        {
            var userId = Guid.NewGuid().ToString();
            var environmentNew = new Environment2D { Name = "A new hope", OwnerUserId = userId};
            _mockAuthenticationService!.Setup(getUserId => getUserId.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockEnvironmentRepo!.Setup(newEnv => newEnv.InsertAsync(environmentNew)).ReturnsAsync(environmentNew);

            var result = await _environment2dController!.Add(environmentNew);
            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteResult));
        }

        [TestMethod]
        public async Task Add_CanSuccesfullyCreateObject()
        {
            var userId = Guid.NewGuid().ToString();
            var envId = Guid.NewGuid();
            var newObject = new Object2D
            {
                PositionX = Random.Shared.Next(1, 20),
                PositionY = Random.Shared.Next(1, 20),
                PrefabId = Convert.ToString(Random.Shared.Next(0, 4)),
                RotationZ = Random.Shared.Next(0, 360),
                ScaleX = Random.Shared.Next(1, 5),
                ScaleY = Random.Shared.Next(1, 5),
                SortingLayer = Random.Shared.Next(0,20),
                EnvironmentId = envId
            };
            
            _mockAuthenticationService!.Setup(getUserId => getUserId.GetCurrentAuthenticatedUserId()).Returns(userId);
            _mockObject2dRepo!.Setup(newObj => newObj.InsertAsync(newObject)).ReturnsAsync(newObject);

            var result = await _object2dController!.Add(newObject);

            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteResult));
        }
    }
}
