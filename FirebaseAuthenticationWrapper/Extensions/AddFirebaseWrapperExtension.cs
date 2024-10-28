using System;
using FirebaseAuthenticationWrapper.Common;
using FirebaseAuthenticationWrapper.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace FirebaseAuthenticationWrapper.Extensions;

public static class AddFirebaseWrapperExtension
{
    public static IServiceCollection AddFirebaseAuthenticationService(
        this IServiceCollection services
    )
    {
        services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, FirebaseAuthHandler>(
                JwtBearerDefaults.AuthenticationScheme,
                (o) => { }
            );

        services.AddTransient<IFirebaseTokenFactory, FirebaseTokenFactory>();
        services.AddScoped<FirebaseAuthFunctionHandler>();

        return services;
    }
}
