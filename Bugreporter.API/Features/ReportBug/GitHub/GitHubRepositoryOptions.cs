using System;

namespace Bugreporter.API.Features.ReportBug.GitHub;

public class GitHubRepositoryOptions
{
    public string? Owner { get; set; }
    public string? Name { get; set; }
}
