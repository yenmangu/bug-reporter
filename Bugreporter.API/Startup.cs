// using System;
// using Microsoft.Azure.Functions.Extensions.DependencyInjection;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;

// [assembly: FunctionsStartup(typeof(Bugreporter.API.Startup))]

// namespace Bugreporter.API;

// public class Startup : FunctionsStartup
// {
//     public override void Configure(IFunctionsHostBuilder builder)
//     {
//         builder.Services.AddSingleton<HelloWorld>();
//     }
// }

// public class HelloWorld
// {
//     private readonly ILogger<HelloWorld> _logger;

//     public HelloWorld(ILogger<HelloWorld> logger)
//     {
//         _logger = logger;
//     }

//     public void Run()
//     {
//         _logger.LogInformation("Hello World");
//     }
// }
