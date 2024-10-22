using System.Text.Json;
using Bugreporter.API.Features.ReportBug;
using Bugreporter.API.Features.ReportBug.GitHub;
using Bugreporter.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
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
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "bugs")] HttpRequestData req
        // ReportBugRequest req
        )
        {
            try
            {
                string? reqBody = await new StreamReader(req.Body).ReadToEndAsync();

                var reportBugRequest = JsonSerializer.Deserialize<ReportBugRequest>(
                    reqBody,
                    JsonSerializerSettings.Options
                );
                NewBug? newBug;
                if (reportBugRequest != null)
                {
                    newBug = new NewBug(reportBugRequest.Summary, reportBugRequest.Description);
                    // newBug = new NewBug(req.Summary, req.Description);
                }
                else
                {
                    throw new InvalidOperationException("Expecting body");
                }

                // We await a reported bug back from GitHub
                // To do so, we need to pass the newBug into the Execute function
                ReportedBug reportedBug = await _createGitHubIssueCommand.Execute(newBug);

                return new OkObjectResult(
                    new ReportBugResponse()
                    {
                        Id = reportedBug.Id,
                        summary = reportedBug.Summary,
                        description = reportedBug.Description
                    }
                );
            }
            catch (InvalidOperationException ex)
            {
                string message = "Body not found??";
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
