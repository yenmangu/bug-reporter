using System.Text.Json;
using Bugreporter.API.Features.ReportBug;
using Bugreporter.API.Features.ReportBug.GitHub;
using Bugreporter.API.Helpers;
using FirebaseAuthenticationWrapper.Models;
using FirebaseAuthenticationWrapper.Services;
using Microsoft.AspNetCore.Authentication;
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
        private readonly FirebaseAuthFunctionHandler _authenticationHandler;

        //
        private readonly ILogger<ReportBugFunction> _logger;

        public ReportBugFunction(
            CreateGitHubIssueCommand createGitHubIssueCommand,
            FirebaseAuthFunctionHandler authenticationHandler,
            ILogger<ReportBugFunction> logger
        )
        {
            _createGitHubIssueCommand = createGitHubIssueCommand;
            _authenticationHandler = authenticationHandler;
            _logger = logger;
        }

        [Function("ReportBugFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "bugs")] HttpRequestData req,
            HttpRequest httpRequest
        // ReportBugRequest req
        )
        {
            try
            {
                string? reqBody = await new StreamReader(req.Body).ReadToEndAsync();

                // Authenticate
                AuthenticateResult authenticationResult =
                    await _authenticationHandler.HandleAuthenticateAsync(httpRequest);

                if (!authenticationResult.Succeeded)
                {
                    return new UnauthorizedResult();
                }

                string userId;
                var claimId = authenticationResult.Principal.FindFirst(UserClaimType.ID);
                if (claimId != null)
                {
                    userId = claimId.Value;
                    _logger.LogInformation("Authenticated User {userId}", userId);
                }

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
