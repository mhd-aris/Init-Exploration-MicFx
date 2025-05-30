using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;

namespace MicFx.Tests.Integration.Controllers;

public class HelloWorldControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HelloWorldControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_RootPath_ReturnsSuccessOrRedirect()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert - Orchard Core may redirect to setup page
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Get_HelloWorldTest_ReturnsJsonResponse()
    {
        // Act
        var response = await _client.GetAsync("/Hello/test");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("application/json", response.Content.Headers.ContentType?.ToString());
        
        // Verify JSON structure
        var jsonDoc = JsonDocument.Parse(content);
        Assert.True(jsonDoc.RootElement.TryGetProperty("message", out var messageProperty));
        Assert.Equal("Hello from MicFx!", messageProperty.GetString());
    }
} 