using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FirebaseAuthenticationWrapper.Services;

public class FirebaseAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly FirebaseAuthFunctionHandler _functionHandler;

    public FirebaseAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        TimeProvider clock,
        FirebaseAuthFunctionHandler functionHandler
    )
        : base(options, logger, encoder)
    {
        _functionHandler = functionHandler;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // return Task.FromResult(AuthenticateResult.NoResult());
        // return Task.FromResult(AuthenticateResult.Success( ));
        // return Task.FromResult(AuthenticateResult.NoResult());
        return _functionHandler.HandleAuthenticateAsync(Context);
    }
}
