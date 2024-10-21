using System.Text.Json;
using Bugreporter.API.Features.ReportBug;
using Bugreporter.API.Features.ReportBug.GitHub;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Bugreporter.API.Functions
{
    public class ReportBugFunction
    {
        private CreateGitHubIssueCommand _createGitHubIssueCommand;
        private readonly ILogger<ReportBugFunction> _logger;

        public ReportBugFunction(
            ILogger<ReportBugFunction> logger,
            CreateGitHubIssueCommand createGitHubIssueCommand
        )
        {
            _logger = logger;
            _createGitHubIssueCommand = createGitHubIssueCommand;
        }

        [Function("ReportBugFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "bugs")] HttpRequest req
        )
        {
            try
            {
                NewBug newBug = new NewBug(
                    "Very bad bug",
                    "The div on the home screen is not centered."
                );

                // We await a reported bug back from GitHub
                // To do so, we need to pass the newBug into the Execute function
                ReportedBug reportedBug = await _createGitHubIssueCommand.Execute(newBug);

                return new OkObjectResult(reportedBug);
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
