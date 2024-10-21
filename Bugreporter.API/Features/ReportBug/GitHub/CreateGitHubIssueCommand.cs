using System;
using Microsoft.Extensions.Logging;

namespace Bugreporter.API.Features.ReportBug.GitHub;

public class CreateGitHubIssueCommand
{
    private readonly ILogger<CreateGitHubIssueCommand> _logger;

    public CreateGitHubIssueCommand(ILogger<CreateGitHubIssueCommand> logger)
    {
        _logger = logger;
    }

    public async Task<ReportedBug> Execute(NewBug newBug)
    {
        _logger.LogInformation("Creating GitHub Issue");
        // Create GetHubIssue
        // Just return a test bug for now
        ReportedBug reportedBug = new ReportedBug(
            "1",
            "Very bad bug",
            "The div on the home screen is not centered."
        );

        _logger.LogInformation($"Successfully Created GitHub Issue (ID): {reportedBug.Id}");

        return reportedBug;
    }
}
