using System;

namespace FirebaseAuthenticationWrapper.Common;

public class MockFirebaseToken : IFirebaseToken
{
    public IReadOnlyDictionary<string, object> Claims { get; }

    public MockFirebaseToken(IReadOnlyDictionary<string, object> claims)
    {
        Claims = claims;
    }
}
