using Bugreporter.API.Features.ReportBug.GitHub;
using Bugreporter.API.Functions;
using Bugreporter.API.Helpers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(
        (context, services) =>
        {
            services.Configure<GitHubRepositoryOptions>(o =>
            {
                o.Name = context.Configuration.GetValue<string>("GITHUB_REPO_NAME");
                o.Owner = context.Configuration.GetValue<string>("GITHUB_REPO_OWNER");
            });
            string? gitHubToken = context.Configuration["GITHUB_TOKEN"];
            services.AddSingleton(new GitHubClientFactory(gitHubToken));
            services.AddSingleton<CreateGitHubIssueCommand>();
            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();
        }
    )
    .Build();

host.Run();
