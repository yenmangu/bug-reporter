using System;
using FirebaseAuthenticationWrapper.Common;
using FirebaseAuthenticationWrapper.Services;

namespace FirebaseAuthenticationWrapper.Tests;

[TestClass]
public class FirebaseTokenAdapter_Test
{
    private FirebaseTokenAdapter _adapter;
    private IReadOnlyDictionary<string, object> _mockClaims;

    [TestInitialize]
    public void Setup()
    {
        _mockClaims = new Dictionary<string, object>
        {
            { "user_id", "ID_123" },
            { "email", "user@example.com" },
            { "email_verified", true },
            { "name", "User123" }
        };
    }

    public void InitialiseAdapter(IReadOnlyDictionary<string, object> mockClaims)
    {
        _adapter = new FirebaseTokenAdapter(new Common.MockFirebaseToken(_mockClaims));
    }

    [TestMethod]
    public void TestFirebaseTokenAdapter_ReturnsNotNull_MockClaims()
    {
        InitialiseAdapter(_mockClaims);
        Assert.IsNotNull(_adapter);
    }

    [TestMethod]
    public void TestFirebaseTokenAdapter_ReturnsClaimsDictionary_MockClaims()
    {
        InitialiseAdapter(_mockClaims);
        Assert.AreEqual("ID_123", _adapter.Claims["user_id"]);
        Assert.AreEqual("user@example.com", _adapter.Claims["email"]);
        Assert.AreEqual(true, _adapter.Claims["email_verified"]);
        Assert.AreEqual("User123", _adapter.Claims["name"]);
    }
}

// IFirebaseToken must be fully qualified to be recognised as the same type across different files
public class MockFirebaseToken : IFirebaseToken
{
    public IReadOnlyDictionary<string, object> Claims { get; }

    public MockFirebaseToken(IReadOnlyDictionary<string, object> claims)
    {
        Claims = claims;
    }
}
