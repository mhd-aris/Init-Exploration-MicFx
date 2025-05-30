using Microsoft.AspNetCore.Mvc;
using MicFx.Module.HelloWorld.Controllers;

namespace MicFx.Module.HelloWorld.Tests.Controllers;

public class HelloWorldControllerTests
{
    private readonly HelloWorldController _controller;

    public HelloWorldControllerTests()
    {
        _controller = new HelloWorldController();
    }

    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Test_ReturnsJsonResult()
    {
        // Act
        var result = _controller.Test();

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var value = jsonResult.Value;
        
        // Verify the anonymous object properties
        var messageProperty = value?.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal("Hello from MicFx!", messageProperty.GetValue(value));
    }
} 