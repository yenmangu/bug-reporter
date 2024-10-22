using System;
using Bugreporter.API.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Octokit;

namespace Bugreporter.API.Features.ReportBug.GitHub;

public class CreateGitHubIssueCommand
{
    private readonly GitHubClientFactory _gitHubClientFactory;
    private readonly GitHubRepositoryOptions _gitHubRepositoryOptions;
    private readonly ILogger<CreateGitHubIssueCommand> _logger;

    public CreateGitHubIssueCommand(
        ILogger<CreateGitHubIssueCommand> logger,
        GitHubClientFactory gitHubClientFactory,
        IOptions<GitHubRepositoryOptions> gitHubRepositoryOptions
    )
    {
        _logger = logger;
        _gitHubClientFactory = gitHubClientFactory;
        _gitHubRepositoryOptions = gitHubRepositoryOptions.Value;
    }

    public async Task<ReportedBug> Execute(NewBug newBug)
    {
        _logger.LogInformation("Creating GitHub Issue");

        NewIssue newIssue = new NewIssue(newBug.Summary) { Body = newBug.Summary };
        var client = _gitHubClientFactory.CreateClient();
        string? repo = _gitHubRepositoryOptions.Name;
        string? owner = _gitHubRepositoryOptions.Owner;
        Issue createdIssue = await client.Issue.Create(owner, repo, newIssue);

        _logger.LogInformation(
            $"Successfully Created GitHub Issue (Number): {createdIssue.Number}"
        );

        return new ReportedBug(
            createdIssue.Number.ToString(),
            createdIssue.Title,
            createdIssue.Body
        );
        ;
    }
}
