using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bugreporter.API.Functions
{
    public class ReportBugFunction
    {
        private readonly ILogger<ReportBugFunction> _logger;

        public ReportBugFunction(ILogger<ReportBugFunction> logger)
        {
            _logger = logger;
        }

        [Function("ReportBugFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "bugs")] HttpRequest req
        )
        {
            try
            {
                // _helloWorld.Run();
                //     _logger.LogInformation("C# HTTP trigger function processed a request.");
                //     string? name = req.Query["name"];
                //     string? requstBody = await new StreamReader(req.Body).ReadToEndAsync();
                //     JsonElement data = JsonSerializer.Deserialize<JsonElement>(requstBody);

                //     if (data.TryGetProperty("name", out JsonElement nameElement))
                //     {
                //         name = nameElement.GetString();
                //     }
                //     else
                //     {
                //         throw new InvalidOperationException("name property not found");
                //     }

                return new OkObjectResult($"");
            }
            catch (InvalidOperationException ex)
            {
                string message = "Required property 'name' not found";
                _logger.LogError(ex, message);
                return new BadRequestObjectResult(message);
            }
            catch (Exception ex)
            {
                string message = "An unexpected error occurred";
                _logger.LogError(ex, message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
