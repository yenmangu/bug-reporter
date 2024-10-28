using System;
using FirebaseAdmin.Auth;
using FirebaseAuthenticationWrapper.Common;

namespace FirebaseAuthenticationWrapper.Services;

// Implements the IFirebaseToken interface;
// provides the <string, object> Claims contract for the interface
public class FirebaseTokenAdapter : IFirebaseToken
{
    private readonly Common.IFirebaseToken _firebaseToken;
    private readonly IReadOnlyDictionary<string, object> _claims;

    public FirebaseTokenAdapter(Common.IFirebaseToken firebaseToken)
    {
        _claims = firebaseToken.Claims;
    }

    public FirebaseTokenAdapter(FirebaseToken firebaseToken)
    {
        _claims = firebaseToken.Claims;
    }

    // Expression-bodied property => syntax returns the _claims member through the 'Claims' get accessor
    public IReadOnlyDictionary<string, object> Claims => _claims;
}
