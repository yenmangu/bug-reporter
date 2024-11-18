// using DotNet.Meteor.HotReload.Plugin;

using System;
using Bugreporter.Client.Features.ReportBug;
using Bugreporter.Client.Features.ReportBug.API;
using Bugreporter.Client.Pages.ReportBug;
using Bugreporter.Client.Pages.SignIn;
using Bugreporter.Client.Pages.SignUp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Refit;

namespace Bugreporter.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(
                fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                }
            );
        builder.Services.AddRefitClient<IReportBugApiCommand>()
            .ConfigureHttpClient(
                c => c.BaseAddress = new Uri("http://localhost:7071/api")
            );
        builder.Services.AddTransient<ReportBugViewModel>();
        builder.Services.AddTransient<ReportBugFormViewModel>();
        builder.Services.AddTransient<ReportBugView>(
            s => new ReportBugView(
                s.GetRequiredService<ReportBugViewModel>()
            )
        );
        builder.Services.AddTransient<SignInViewModel>();
        builder.Services.AddTransient<SignInView>(
            s => new SignInView(
                s.GetRequiredService<SignInViewModel>()
            )
        );
        builder.Services.AddTransient<SignUpViewModel>();
        builder.Services.AddTransient<SignUpView>(
            s => new SignUpView(
                s.GetRequiredService<SignUpViewModel>()
            )
        );
#if DEBUG
        builder.Logging.AddDebug();
        // MauiAppBuilder mauiAppBuilder = builder.EnableHotReload();
#endif

        return builder.Build();
    }
}