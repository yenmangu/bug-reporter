using System;
using Octokit;

namespace Bugreporter.API.Helpers;

public class GitHubClientFactory
{
    private readonly string? _authToken;

    public GitHubClientFactory(string? authToken)
    {
        _authToken = authToken;
    }

    public GitHubClient CreateClient()
    {
        var client = new GitHubClient(new ProductHeaderValue("Bugreporter-API"))
        {
            Credentials = new Credentials(_authToken)
        };
        return client;
    }
}
