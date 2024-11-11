// using DotNet.Meteor.HotReload.Plugin;

using Bugreporter.Client.Pages.ReportBug;
using Bugreporter.Client.Pages.SignIn;
using Bugreporter.Client.Pages.SignUp;
using Microsoft.Extensions.Logging;

namespace Bugreporter.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        builder.Services.AddTransient<ReportBugViewModel>();
        builder.Services.AddTransient<ReportBugView>(s => new ReportBugView(
            s.GetRequiredService<ReportBugViewModel>()
        ));
        builder.Services.AddTransient<SignInViewModel>();
        builder.Services.AddTransient<SignInView>(s => new SignInView(
            s.GetRequiredService<SignInViewModel>()));
        builder.Services.AddTransient<SignUpViewModel>();
        builder.Services.AddTransient<SignUpView>(s => new SignUpView(
            s.GetRequiredService<SignUpViewModel>()));
#if DEBUG
        builder.Logging.AddDebug();
        // MauiAppBuilder mauiAppBuilder = builder.EnableHotReload();
#endif

        return builder.Build();
    }
}