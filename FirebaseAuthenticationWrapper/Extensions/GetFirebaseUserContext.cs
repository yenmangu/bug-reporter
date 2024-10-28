using System;
using System.Security.Claims;
using FirebaseAuthenticationWrapper.Models;
using Microsoft.AspNetCore.Http;

namespace FirebaseAuthenticationWrapper.Extensions;

public static class GetFirebaseUserContext
{
    public static FirebaseUserModel GetFirebaseUser(this HttpContext httpContext)
    {
        ClaimsPrincipal claimsPrincipal = httpContext.User;
        string id = claimsPrincipal.FindFirstValue(UserClaimType.ID);
        string email = claimsPrincipal.FindFirstValue(UserClaimType.EMAIL);
        string username = claimsPrincipal.FindFirstValue(UserClaimType.USERNAME);
        bool.TryParse(
            claimsPrincipal.FindFirstValue(UserClaimType.EMAIL_VERIFIED),
            out bool emailVerified
        );

        return new FirebaseUserModel(id, email, username, emailVerified);
    }
}
