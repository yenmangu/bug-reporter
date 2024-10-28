using System;
using System.Security.Claims;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAuthenticationWrapper;
using FirebaseAuthenticationWrapper.Common;
using FirebaseAuthenticationWrapper.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;

namespace FirebaseAuthenticationWrapper.Services;

public class FirebaseAuthFunctionHandler
{
    private const string BEARER_PREFIX = "Bearer ";
    protected readonly FirebaseApp _firebaseApp;
    public readonly IFirebaseTokenFactory _tokenFactory;

    public FirebaseAuthFunctionHandler(IFirebaseTokenFactory tokenFactory, FirebaseApp firebaseApp)
    {
        _tokenFactory = tokenFactory;
        _firebaseApp = firebaseApp;
    }

    public Task<AuthenticateResult> HandleAuthenticateAsync(HttpRequest request) =>
        HandleAuthenticateAsync(request.HttpContext);

    public async Task<AuthenticateResult> HandleAuthenticateAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.NoResult();
        }

        string bearerToken = context.Request.Headers.Authorization;

        if (bearerToken == null || !bearerToken.StartsWith(BEARER_PREFIX))
        {
            return AuthenticateResult.Fail("Invalid scheme");
        }

        string token = bearerToken.Substring(BEARER_PREFIX.Length);

        if (token == null || token.Length == 0)
        {
            return AuthenticateResult.Fail("Invalid token");
        }

        return await HandleFirebaseAuth(token);
    }

    public async Task<AuthenticateResult> HandleFirebaseAuth(string token)
    {
        try
        {
            FirebaseToken firebaseToken = await FirebaseAuth
                .GetAuth(_firebaseApp)
                .VerifyIdTokenAsync(token);

            var tokenAdapter = _tokenFactory.Create(firebaseToken);

            var ticket = CreateAuthTicket(tokenAdapter);
            return AuthenticateResult.Success(ticket);
        }
        catch (System.Exception ex)
        {
            return AuthenticateResult.Fail(ex);
        }
    }

    protected AuthenticationTicket CreateAuthTicket(Common.IFirebaseToken token)
    {
        if (token == null)
        {
            CreateAuthTicket();
        }

        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(
            new List<ClaimsIdentity>()
            {
                new ClaimsIdentity(ToClaims(token.Claims), nameof(ClaimsIdentity))
            }
        );

        return new AuthenticationTicket(claimsPrincipal, JwtBearerDefaults.AuthenticationScheme);
    }

    protected AuthenticateResult CreateAuthTicket()
    {
        return AuthenticateResult.Fail("No token provided");
    }

    protected IEnumerable<Claim> ToClaims(IReadOnlyDictionary<string, object> claims)
    {
        return new List<Claim>
        {
            new Claim(UserClaimType.ID, claims.GetValueOrDefault("user_id", "").ToString()),
            new Claim(UserClaimType.EMAIL, claims.GetValueOrDefault("email", "").ToString()),
            new Claim(UserClaimType.USERNAME, claims.GetValueOrDefault("name", "").ToString()),
            new Claim(
                UserClaimType.EMAIL_VERIFIED,
                claims.GetValueOrDefault("email_verified", "").ToString()
            )
        };
    }
}
