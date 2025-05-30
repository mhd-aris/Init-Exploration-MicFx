using MicFx.Module.HelloWorld.Services;

namespace MicFx.Tests.Unit.Modules.HelloWorld.Services;

public class GreeterServiceTests
{
    private readonly GreeterService _greeterService;

    public GreeterServiceTests()
    {
        _greeterService = new GreeterService();
    }

    [Fact]
    public void Greet_WithValidName_ReturnsExpectedGreeting()
    {
        // Arrange
        var name = "MicFx";
        var expectedGreeting = "Hello, MicFx!";

        // Act
        var result = _greeterService.Greet(name);

        // Assert
        Assert.Equal(expectedGreeting, result);
    }

    [Theory]
    [InlineData("John", "Hello, John!")]
    [InlineData("Alice", "Hello, Alice!")]
    [InlineData("World", "Hello, World!")]
    public void Greet_WithDifferentNames_ReturnsCorrectGreeting(string name, string expected)
    {
        // Act
        var result = _greeterService.Greet(name);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Greet_WithEmptyString_ReturnsGreetingWithEmptyName()
    {
        // Arrange
        var name = "";
        var expectedGreeting = "Hello, !";

        // Act
        var result = _greeterService.Greet(name);

        // Assert
        Assert.Equal(expectedGreeting, result);
    }

    [Fact]
    public void Greet_WithNull_ReturnsGreetingWithNull()
    {
        // Arrange
        string name = null!;
        var expectedGreeting = "Hello, !";

        // Act
        var result = _greeterService.Greet(name);

        // Assert
        Assert.Equal(expectedGreeting, result);
    }
} 