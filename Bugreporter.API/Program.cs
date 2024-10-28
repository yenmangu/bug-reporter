using Bugreporter.API.Features.ReportBug.GitHub;
using Bugreporter.API.Functions;
using Bugreporter.API.Helpers;
using FirebaseAdmin;
using FirebaseAuthenticationWrapper.Extensions;
using Google.Apis.Auth.OAuth2;
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
            services.AddFirebaseAuthenticationService();
            _ = services.AddSingleton<FirebaseApp>(provider =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                string? firebaseConfig = configuration.GetValue<string>("FIREBASE_CONFIG");
                if (firebaseConfig != null)
                {
                    // return new FirebaseAppFactory(firebaseConfig);
                    var credential = GoogleCredential.FromJson(firebaseConfig);
                    return FirebaseApp.Create(new AppOptions { Credential = credential });
                }
                else
                {
                    throw new ArgumentException("Firebase config not found");
                }
            });
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
