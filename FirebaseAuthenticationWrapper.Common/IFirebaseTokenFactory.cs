using System;
using FirebaseAdmin.Auth;

namespace FirebaseAuthenticationWrapper.Common;

public interface IFirebaseTokenFactory
{
    IFirebaseToken Create(FirebaseToken firebaseToken);
}
