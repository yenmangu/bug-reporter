using System;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Bugreporter.API.Helpers;

public class FirebaseAppFactory
{
    private readonly GoogleCredential _googleCredential;

    public FirebaseAppFactory(string googleCredentialJson)
    {
        _googleCredential = GoogleCredential.FromJson(googleCredentialJson);
    }

    public FirebaseApp CreateFirebaseApp()
    {
        FirebaseApp firebaseApp = FirebaseApp.Create(
            new AppOptions() { Credential = _googleCredential }
        );
        return firebaseApp;
    }
}
