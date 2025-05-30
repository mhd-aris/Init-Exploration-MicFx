namespace MicFx.Module.HelloWorld.Services;

public class GreeterService
{
    public string Greet(string name) => $"Hello, {name ?? string.Empty}!";
}