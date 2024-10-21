using Microsoft.Extensions.Logging;

namespace Bugreporter.API.Functions;

public class HelloWorld
{
    private readonly ILogger<HelloWorld> _logger;

    public HelloWorld(ILogger<HelloWorld> logger)
    {
        _logger = logger;
    }

    public void Run()
    {
        _logger.LogInformation("Hello World");
    }
}
