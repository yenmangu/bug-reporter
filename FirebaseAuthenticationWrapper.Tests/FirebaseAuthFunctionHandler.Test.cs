using System;
using System.Security.Claims;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAuthenticationWrapper.Common;
using FirebaseAuthenticationWrapper.Models;
using FirebaseAuthenticationWrapper.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace FirebaseAuthenticationWrapper.Tests;

[TestClass]
public class FirebaseAuthFunctionHandler_Test
{
    private FirebaseApp _firebaseApp;
    private FirebaseAuthFunctionHandler _functionHandler;
    private HttpContext _httpContext;
    private HttpRequest _httpRequest;
    private FirebaseAuthFunctionHandler_Extension _firebaseExtension;
    private Common.MockFirebaseToken _mockFirebaseToken;
    private string _testBearer;

    private IReadOnlyDictionary<string, object> MockData()
    {
        return new Dictionary<string, object>
        {
            { "user_id", "ID_123" },
            { "name", "User123" },
            { "email", "user123@domain123.com" },
            { "email_verified", "true" }
        };
    }

    [TestInitialize]
    public void Setup()
    {
        _httpContext = new DefaultHttpContext();
        _httpRequest = _httpContext.Request;
        _mockFirebaseToken = new Common.MockFirebaseToken(MockData());
        var tokenFactory = new FirebaseTokenFactory();
        var mockData = MockData();
        var mockFirebaseToken = tokenFactory.CreateMockToken(MockData());
        _firebaseExtension = new FirebaseAuthFunctionHandler_Extension(tokenFactory, _firebaseApp);
        _functionHandler = new FirebaseAuthFunctionHandler(tokenFactory, _firebaseApp);
        _testBearer = "Bearer Test_token";
    }

    private async Task<AuthenticateResult> ReturnResult(HttpContext context)
    {
        var result = await _functionHandler.HandleAuthenticateAsync(context);
        return result;
    }

    public static void AssertBoilerplate(AuthenticateResult result)
    {
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(AuthenticateResult));
    }

    public void SetUpFirebaseExtension()
    {
        _firebaseExtension = new FirebaseAuthFunctionHandler_Extension(
            new FirebaseTokenFactory(),
            _firebaseApp
        );
    }

    [TestMethod]
    public void TestHandler_ShouldReturnNotNull()
    {
        Assert.IsNotNull(_functionHandler);
    }

    [TestMethod]
    public void TestHandler_TokenFactory_ShouldNotReturnNull()
    {
        var mockToken = new Common.MockFirebaseToken(MockData());
        Assert.IsNotNull(_functionHandler._tokenFactory, "Should not be null");
    }

    [TestMethod]
    public async Task TestHandleAuthenticateAsync_Delegation_ShouldReturnEqual()
    {
        var httpContext = new DefaultHttpContext();
        var request = httpContext.Request;

        httpContext.Request.Headers.Authorization = "InvalidScheme token";

        // Act
        var resultFromRequest = await _functionHandler.HandleAuthenticateAsync(request);
        var resultFromContext = await _functionHandler.HandleAuthenticateAsync(httpContext);

        // Assert

        Assert.AreEqual(
            resultFromRequest,
            resultFromContext,
            "HandleAuthenticateAsync(HttpRequest) should delegate to HandleAuthenticateAsync(HttpContext.)"
        );
    }

    [TestMethod]
    public async Task HandleAuthenticateAsync_MissingAuthorizationHeader_ReturnsNoResult()
    {
        var result = await ReturnResult(_httpContext);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(AuthenticateResult));
        Assert.IsFalse(result.Succeeded, "expected auth to not success");
        Assert.IsTrue(result.None, "Expected result not to be true");
    }

    [TestMethod]
    public async Task HandleAuthenticateAsync_InvalidScheme_ReturnsFail()
    // Must return fail if the auth header does not contain 'Bearer'
    {
        _httpContext.Request.Headers.Authorization = "";
        var result = await ReturnResult(_httpContext);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(AuthenticateResult));
        Assert.IsTrue(result.Failure != null, "expected Auth to fail due to invalid scheme");
        Assert.AreEqual("Invalid scheme", result.Failure.Message);
    }

    [TestMethod] // fails if no token
    public async Task HandleAuthenticateAsync_InvalidToken_ReturnsFail_NoToken()
    {
        _httpContext.Request.Headers.Authorization = "Bearer ";
        var result = await ReturnResult(_httpContext);
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(AuthenticateResult));
        Assert.IsTrue(result.Failure != null, "expected Auth to fail due to invalid scheme");
        Assert.AreEqual("Invalid token", result.Failure.Message);
    }

    [TestMethod]
    public async Task HandleAuthenticateAsync_NotAuth_ReturnsFail_NotAuth()
    {
        _httpContext.Request.Headers.Authorization = "Bearer";
        var result = await ReturnResult(_httpContext);

        AssertBoilerplate(result);
        // Assert.IsTrue()
    }

    [TestMethod]
    public void TestClaimsCreation_Empty_ShouldReturnNotNull()
    {
        var claims = _firebaseExtension.ToClaims();

        Assert.IsNotNull(claims);
    }

    [TestMethod]
    public void TestClaimsCreation_WithData_ShouldReturnClaims()
    {
        SetUpFirebaseExtension();
        var mockData = MockData();
        var claims = _firebaseExtension.ToClaims(mockData);

        Assert.IsNotNull(claims);
        var claimsList = claims.ToList();

        Assert.IsTrue(claimsList.Any(c => c.Type == UserClaimType.ID && c.Value == "ID_123"));
        Assert.IsTrue(
            claimsList.Any(c => c.Type == UserClaimType.USERNAME && c.Value == "User123")
        );
        Assert.IsTrue(
            claimsList.Any(c => c.Type == UserClaimType.EMAIL && c.Value == "user123@domain123.com")
        );
        Assert.IsTrue(
            claimsList.Any(c => c.Type == UserClaimType.EMAIL_VERIFIED && c.Value == "true")
        );
    }

    [TestMethod]
    public void TestAuthFromToken_SouldReturnNotNull()
    {
        SetUpFirebaseExtension();
        var authTicket = _firebaseExtension.CreateAuthTicket();

        Assert.IsNotNull(authTicket);
    }

    [TestMethod]
    public void TestAuthTicketFromToken_EmptyClaims_ShouldReturnFail()
    {
        SetUpFirebaseExtension();
        AuthenticationTicket authTicket = _firebaseExtension.CreateAuthTicket(_mockFirebaseToken);
        Assert.IsNotNull(authTicket);
        Assert.IsInstanceOfType(authTicket, typeof(AuthenticationTicket));
    }

    // [TestMethod]
    // public async Task TestHandleFirebaseAuth_MockToken_ShouldReturn_SuccessTicket()
    // {
    //     SetUpFirebaseExtension();

    //     var result = await _functionHandler.HandleFirebaseAuth(_testBearer);

    //     // Result
    //     Assert.IsNotNull(result, "Result should not be null");
    //     Assert.IsTrue(result.Succeeded, "Authentication should succeed");

    //     // Ticket
    //     Assert.IsNotNull(result.Ticket, "Ticket should not be null");
    //     Assert.IsInstanceOfType(result.Ticket, typeof(AuthenticationTicket));

    //     // Claims identity casting
    //     var claimsIdentity = result.Ticket.Principal.Identity as ClaimsIdentity;

    //     Assert.IsNotNull(claimsIdentity, "ClaimsIdentity should not be null");
    //     Assert.AreEqual(
    //         "ID_123",
    //         claimsIdentity.FindFirst("id")?.Value,
    //         "User ID claim should match"
    //     );
    //     Assert.AreEqual(
    //         "user@example.com",
    //         claimsIdentity.FindFirst("email")?.Value,
    //         "User email claim should match"
    //     );
    //     Assert.AreEqual(
    //         "User123",
    //         claimsIdentity.FindFirst("username")?.Value,
    //         "User email claim should match"
    //     );
    //     Assert.AreEqual(
    //         "True",
    //         claimsIdentity.FindFirst("email_verified")?.Value,
    //         "User email claim should match"
    //     );
    // }
}

public class FirebaseAuthFunctionHandler_Extension : FirebaseAuthFunctionHandler
{
    public IDictionary<string, object> testApiToken;
    public Common.IFirebaseToken mockFirebaseToken;
    public IFirebaseTokenFactory _tokenFactory;
    public FirebaseApp _firebaseeApp;

    public FirebaseAuthFunctionHandler_Extension(
        IFirebaseTokenFactory tokenFactory,
        FirebaseApp firebaseApp
    )
        : base(tokenFactory, firebaseApp)
    {
        _tokenFactory = tokenFactory;
    }

    public FirebaseApp GetFirebaseApp()
    {
        return _firebaseApp;
    }

    public List<Claim> CreateClaimsPrincipal()
    {
        var claimsPrincipal = new List<Claim>
        {
            new Claim(UserClaimType.ID, "ID_123"),
            new Claim(UserClaimType.USERNAME, "User123"),
            new Claim(UserClaimType.EMAIL, "user123@domain123.com"),
            new Claim(UserClaimType.EMAIL_VERIFIED, "true")
        };
        return claimsPrincipal;
    }

    public void CreateApiToken()
    {
        testApiToken = new Dictionary<string, object>
        {
            { "kind", "identitytoolkit#SignupNewUserResponse" },
            {
                "idToken",
                "eyJhbGciOiJSUzI1NiIsImtpZCI6IjcxOGY0ZGY5MmFkMTc1ZjZhMDMwN2FiNjVkOGY2N2YwNTRmYTFlNWYiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vYnVnLXJlcG9ydGVyLTM1NzUyIiwiYXVkIjoiYnVnLXJlcG9ydGVyLTM1NzUyIiwiYXV0aF90aW1lIjoxNzI5OTkwMzE0LCJ1c2VyX2lkIjoiR3Y5T3hzT0ROaFNLeHlBWHFjaGt0Sm1PMkVlMiIsInN1YiI6Ikd2OU94c09ETmhTS3h5QVhxY2hrdEptTzJFZTIiLCJpYXQiOjE3Mjk5OTAzMTQsImV4cCI6MTcyOTk5MzkxNCwiZW1haWwiOiJyb2JlcnQuc2hlbGZvcmRAZ29vZ2xlbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsicm9iZXJ0LnNoZWxmb3JkQGdvb2dsZW1haWwuY29tIl19LCJzaWduX2luX3Byb3ZpZGVyIjoicGFzc3dvcmQifX0.9m00w-sbueCAWhMKiz2gEw2pWKhWSPUaE9EQDjBsQmYvTOnCXyaOYqj2-ZbAYLe_-gRwOLbOSoKq9p_02sbgxwDDzRNEP1PYCWVbzYu-fniZks9uVfTVnfRTH-mZi5ykjc7ZPXmsoGW86iSP2RMU5VCrfChJvq4_Zlpfhg48jicaCMLkt03aey_wPOkWwWC9JHTLMvrrD9iK0tW3Pg0Cl7gfR-7LPYw8YyGeRzYntCbfVowzAglcRRIj6wZGPfTuSkKTulVo7YsW6-r9pR406nIVa7ln6Yw9YuX_DtQUjIdrIqHhGIxMhNjw3uWxIqAjtp9qrRGXhfDwTnahJdX6Xw"
            },
            { "email", "robert.shelford@googlemail.com" },
            {
                "refreshToken",
                "AMf-vBwKpP2jI-6BWofzD8PZIh_J5bcEQ4O5-UUYLfFeh95dR_QMEdnSksDwSmDTXOVI-z9xMpLRxexV1BkOTJuCCOE6YLB5TCyOumhEOht_GFNnxOb2s0QRJeULt6o5EXA75KrKr5syyrMxDaaYf6cjV58WEpcGxrwTemE6TDctlgdxQQyDHFVJtmP4DN6Je0U6NAVjYlJXN-zbTNtIFF3clSAfxNVDe7eJ4NL-2RCS0JJOqhfGPwo"
            },
            { "expiresIn", "3600" },
            { "localId", "Gv9OxsODNhSKxyAXqchktJmO2Ee2" }
        };
    }

    public IEnumerable<Claim> ToClaims(IReadOnlyDictionary<string, object> claims) =>
        base.ToClaims(claims);

    public IEnumerable<Claim> ToClaims() => base.ToClaims(new Dictionary<string, object>());

    public AuthenticationTicket CreateAuthTicket(Common.IFirebaseToken token) =>
        base.CreateAuthTicket(token);

    public AuthenticateResult CreateAuthTicket() => base.CreateAuthTicket();
}
