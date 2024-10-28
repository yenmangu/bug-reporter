using System;
using FirebaseAdmin.Auth;
using FirebaseAuthenticationWrapper.Common;

namespace FirebaseAuthenticationWrapper.Services;

public class FirebaseTokenFactory : IFirebaseTokenFactory
{
    public IFirebaseToken Create(FirebaseToken firebaseToken)
    {
        return new FirebaseTokenAdapter(firebaseToken);
    }

    public IFirebaseToken CreateMockToken(IReadOnlyDictionary<string, object> claims)
    {
        return new MockFirebaseToken(claims);
    }
}
