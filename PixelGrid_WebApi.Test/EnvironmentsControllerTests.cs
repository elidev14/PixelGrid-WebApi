using Microsoft.AspNetCore.Mvc;
using Moq;
using PixelGrid_WebApi.Controllers;
using PixelGrid_WebApi.Datamodels;
using PixelGrid_WebApi.Services;


[TestClass]
public class Environment2DControllerTests
{
    [TestMethod]
    public async Task AddEnvironment2D_ReturnsBadRequest_WhenEnvironmentIsNull() 
    {
        // Arrange
        var mockSqlEnvironment2DService = new Mock<ISqlEnvironment2DService>();
        var mockAuthService = new Mock<IAuthenticationService>();
        var controller = new Environment2DController(mockSqlEnvironment2DService.Object, mockAuthService.Object);

        // Act
        var response = await controller.AddEnvironment2D(null);

        // Assert
        Assert.IsInstanceOfType(response, typeof(BadRequestObjectResult)); 
        var badRequestResult = (BadRequestObjectResult)response; 
        Assert.AreEqual("Invalid environment data.", badRequestResult.Value);
    }

    [TestMethod]
    public async Task AddEnvironment2D_ReturnsConflict_WhenEnvironmentAlreadyExists() 
    {
        // Arrange
        var environment = new Environment2D { Name = "TestEnvironment", OwnerUserId = "User1" };
        var mockSqlEnvironment2DService = new Mock<ISqlEnvironment2DService>();

        // het opzetten van de UserId want anders gaat het sowieso de code niet doorheen want de GetCurrentAuthenticatedUserId() wordt opgevraagd
        var mockAuthService = new Mock<IAuthenticationService>();
        mockAuthService.Setup(a => a.GetCurrentAuthenticatedUserId()).Returns("User1");

        // Simuleer al bestaande omgeving
        mockSqlEnvironment2DService.Setup(s => s.GetListOfDataAsync("User1"))
            .ReturnsAsync(new List<Environment2D> { new Environment2D { Name = "TestEnvironment", OwnerUserId = "User1" } });

        var controller = new Environment2DController(mockSqlEnvironment2DService.Object, mockAuthService.Object);

        // Act
        var result = await controller.AddEnvironment2D(environment);

        // Assert
        Assert.IsInstanceOfType<ConflictObjectResult>(result); 
        var conflictResult = (ConflictObjectResult)result; 

        // Vergelijk de anonieme objecten als stringrepresentaties
        var expected = new { message = $"An environment with the name '{environment.Name}' already exists." }.ToString();
        var actual = conflictResult.Value.ToString();

        Assert.AreEqual(expected, actual);
    }



    [TestMethod]
    public async Task AddEnvironment2D_ReturnsOk_WhenEnvironmentIsAdded() 
    {
        // Arrange
        var environment = new Environment2D { Name = "NewEnvironment", OwnerUserId = "User1" };
        var mockSqlEnvironment2DService = new Mock<ISqlEnvironment2DService>();
        var mockAuthService = new Mock<IAuthenticationService>();


        mockAuthService.Setup(a => a.GetCurrentAuthenticatedUserId()).Returns("User1");


        mockSqlEnvironment2DService.Setup(s => s.GetListOfDataAsync("User1"))
            .ReturnsAsync(new List<Environment2D>());


        mockSqlEnvironment2DService.Setup(s => s.InsertDataAsync(It.IsAny<Environment2D>())).Returns(Task.CompletedTask);
        var controller = new Environment2DController(mockSqlEnvironment2DService.Object, mockAuthService.Object);

        // Act
        var response = await controller.AddEnvironment2D(environment);

        // Assert
        Assert.IsInstanceOfType<OkObjectResult>(response);  
        var okResult = (OkObjectResult)response;  
        var addedEnvironment = okResult.Value as Environment2D;
        Assert.IsNotNull(addedEnvironment);
        Assert.AreEqual(environment, addedEnvironment);
    }


    [TestMethod]
    public async Task AddEnvironment2D_ReturnsBadRequest_WhenUserExceedsMaxWorlds()
    {
        // Arrange: Maak mockgegevens voor een gebruiker met 5 werelden
        var userId = "user123";
        var mockEnvironments = new List<Environment2D>
    {
        new Environment2D { OwnerUserId = userId, Name = "World 1" },
        new Environment2D { OwnerUserId = userId, Name = "World 2" },
        new Environment2D { OwnerUserId = userId, Name = "World 3" },
        new Environment2D { OwnerUserId = userId, Name = "World 4" },
        new Environment2D { OwnerUserId = userId, Name = "World 5" }
    };

        var mockService = new Mock<ISqlEnvironment2DService>();

        mockService.Setup(s => s.GetListOfDataAsync(It.IsAny<string>()))
            .ReturnsAsync(mockEnvironments);

        var mockAuthService = new Mock<IAuthenticationService>();
        mockAuthService.Setup(a => a.GetCurrentAuthenticatedUserId()).Returns(userId);

        var controller = new Environment2DController(mockService.Object, mockAuthService.Object);

        // Act: Probeer een zesde wereld toe te voegen
        var result = await controller.AddEnvironment2D(new Environment2D { Name = "World 6", OwnerUserId = userId });

        // Assert: Het moet een BadRequest zijn want de limiet is bereikt
        Assert.IsInstanceOfType<BadRequestObjectResult>(result);
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.AreEqual("You have reached the limit of 5 environments", badRequestResult.Value);
    }

    [TestMethod]
    public async Task GetEnvironment2Ds_ReturnsOwnWorlds_WhenUserIsLoggedIn()
    {
        // Arrange: Maak mockgegevens voor werelden
        var userId = "user123";
        var mockEnvironments = new List<Environment2D>
    {
        new Environment2D { OwnerUserId = userId, Name = "World 1" },
        new Environment2D { OwnerUserId = userId, Name = "World 2" }
    };

        var mockService = new Mock<ISqlEnvironment2DService>();
        mockService.Setup(s => s.GetListOfDataAsync(It.IsAny<string>()))
            .ReturnsAsync(mockEnvironments);

        var mockAuthService = new Mock<IAuthenticationService>();
        mockAuthService.Setup(a => a.GetCurrentAuthenticatedUserId()).Returns(userId);

        var controller = new Environment2DController(mockService.Object, mockAuthService.Object);

        // Act: Haal de werelden op voor de user
        var result = await controller.GetEnvironment2Ds();

        // Assert: De gebruiker moet alleen zijn eigen werelden zien
        Assert.IsInstanceOfType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;

        var environments = okResult.Value as List<Environment2D>;
        Assert.AreEqual(2, environments.Count);
        Assert.AreEqual("World 1", environments[0].Name);
        Assert.AreEqual("World 2", environments[1].Name);
    }

    [TestMethod]
    public async Task DeleteEnvironment2D_ReturnsOk_WhenUserDeletesOwnWorld()
    {
        // Arrange: Maak mockgegevens voor een gebruiker en een wereld
        var userId = "user123";
        var environment = new Environment2D { ID = Guid.NewGuid(), Name = "World 1", OwnerUserId = userId };

        var mockService = new Mock<ISqlEnvironment2DService>();
        mockService.Setup(s => s.GetDataAsync(It.IsAny<Guid>())).ReturnsAsync(environment);
        mockService.Setup(s => s.DeleteDataAsync(It.IsAny<Guid>()));

        var mockAuthService = new Mock<IAuthenticationService>();
        mockAuthService.Setup(a => a.GetCurrentAuthenticatedUserId()).Returns(userId);

        var controller = new Environment2DController(mockService.Object, mockAuthService.Object);

        // Act: Verwijder de wereld
        var result = await controller.DeleteEnvironment2D(environment.ID);

        // Assert: De wereld wordt verwijderd

        Assert.IsInstanceOfType<OkObjectResult>(result);

        var okResult = (OkObjectResult)result;
        Assert.AreEqual("Environment2D object deleted", okResult.Value);
    }



}
