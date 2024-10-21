using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bugreporter.API
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<IActionResult> TryRun(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req
        )
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string? name = req.Query["name"];
                string? requstBody = await new StreamReader(req.Body).ReadToEndAsync();
                JsonElement data = JsonSerializer.Deserialize<JsonElement>(requstBody);

                if (data.TryGetProperty("name", out JsonElement nameElement))
                {
                    name = nameElement.GetString();
                }
                else
                {
                    throw new InvalidOperationException("name property not found");
                }

                return new OkObjectResult($"Welcome to Azure Functions! Body: {data}");
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
