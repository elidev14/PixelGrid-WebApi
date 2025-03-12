using Microsoft.AspNetCore.Mvc;
using Moq;
using PixelGrid_WebApi.Controllers;
using PixelGrid_WebApi.Datamodels;
using PixelGrid_WebApi.Services;

[TestClass]
public class Object2DControllerTests
{
    [TestMethod]
    public async Task Add_ReturnsBadRequest_WhenEnvironmentIDIsEmpty() // Arrange, Act, Assert
    {
        // Arrange
        var mockSqlObject2DService = new Mock<ISqlObject2DService>();
        var mockSqlEnvironment2DService = new Mock<ISqlEnvironment2DService>();
        var mockAuthService = new Mock<IAuthenticationService>();
        var controller = new Object2DController(mockSqlObject2DService.Object, mockSqlEnvironment2DService.Object, mockAuthService.Object);

        // Act
        var result = await controller.Add(Guid.Empty, new Object2D());

        // Assert
        Assert.IsInstanceOfType<BadRequestObjectResult>(result); 
        var badRequestResult = (BadRequestObjectResult)result;  // Cast naar het juiste type
        Assert.AreEqual("Invalid environmentID", badRequestResult.Value);
    }

    [TestMethod]
    public async Task Add_ReturnsUnauthorized_WhenUserIsNotOwner() // Arrange, Act, Assert
    {
        // Arrange
        var environmentId = Guid.NewGuid();
        var mockSqlObject2DService = new Mock<ISqlObject2DService>();
        var mockSqlEnvironment2DService = new Mock<ISqlEnvironment2DService>();
        var mockAuthService = new Mock<IAuthenticationService>();

        mockSqlEnvironment2DService.Setup(s => s.GetDataAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Environment2D { ID = environmentId, OwnerUserId = "DifferentUser" });

        mockAuthService.Setup(a => a.GetCurrentAuthenticatedUserId()).Returns("CurrentUser"); // When 'GetCurrentAuthenticatedUserId()' method gets called it will return 'CurrentUser'

        var controller = new Object2DController(mockSqlObject2DService.Object, mockSqlEnvironment2DService.Object, mockAuthService.Object);

        // Act
        var result = await controller.Add(environmentId, new Object2D());

        // Assert
        Assert.IsInstanceOfType<UnauthorizedObjectResult>(result); // Generieke overload
        var unauthorizedResult = (UnauthorizedObjectResult)result;  // Cast naar het juiste type
        Assert.AreEqual("User is not allowed to add the object to the environment", unauthorizedResult.Value);
    }


    [TestMethod]
    public async Task Add_ReturnsOk_WhenObjectIsAddedSuccessfully() // Arrange, Act, Assert
    {
        // Arrange
        var environmentId = Guid.NewGuid();
        var mockSqlObject2DService = new Mock<ISqlObject2DService>();
        var mockSqlEnvironment2DService = new Mock<ISqlEnvironment2DService>();
        var mockAuthService = new Mock<IAuthenticationService>();

        mockSqlEnvironment2DService.Setup(s => s.GetDataAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Environment2D { ID = environmentId, OwnerUserId = "CurrentUser" });

        mockAuthService.Setup(a => a.GetCurrentAuthenticatedUserId()).Returns("CurrentUser");
        mockSqlObject2DService.Setup(s => s.InsertDataAsync(It.IsAny<Object2D>())).Returns(Task.CompletedTask);

        var controller = new Object2DController(mockSqlObject2DService.Object, mockSqlEnvironment2DService.Object, mockAuthService.Object);
        var object2D = new Object2D
        {
            PrefabID = "ValidPrefab",
            PosX = 0,
            PosY = 0,
            ScaleX = 1,
            ScaleY = 1,
            RotationZ = 0,
            SortingLayer = 1
        };

        // Act
        var result = await controller.Add(environmentId, object2D);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(result); // Generieke overload
        var okResult = (OkObjectResult)result;  // Cast naar het juiste type
        var addedObject = okResult.Value as Object2D;
        Assert.IsNotNull(addedObject);
        Assert.AreEqual(object2D, addedObject);
    }

    [TestMethod]
    public async Task DeleteObject2D_ReturnsOk_WhenObjectIsDeleted()
    {
        // Arrange: Maak een mock voor de omgeving en het object
        var userId = "user123";
        var environment = new Environment2D { ID = Guid.NewGuid(), Name = "World 1", OwnerUserId = userId };
        var object2D = new Object2D { ID = Guid.NewGuid(), PrefabID = "Object1", EnvironmentID = environment.ID };

        var mockEnvService = new Mock<ISqlEnvironment2DService>();
        mockEnvService.Setup(s => s.GetDataAsync(It.IsAny<Guid>())).ReturnsAsync(environment);

        var mockObjService = new Mock<ISqlObject2DService>();
        mockObjService.Setup(s => s.DeleteDataAsync(It.IsAny<Guid>(), It.IsAny<Guid>()));

        var mockAuthService = new Mock<IAuthenticationService>();
        mockAuthService.Setup(a => a.GetCurrentAuthenticatedUserId()).Returns(userId);

        var controller = new Object2DController(mockObjService.Object, mockEnvService.Object, mockAuthService.Object);

        // Act: Verwijder het object
        var result = await controller.Delete(environment.ID, object2D.ID);

        // Assert: Het object moet succesvol verwijderd worden
        Assert.IsInstanceOfType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.AreEqual("Object2D deleted successfully.", okResult.Value);
    }
}
