using System;

namespace FirebaseAuthenticationWrapper.Models;

public class FirebaseUserModel
{
    public string Id { get; }
    public string Email { get; }
    public string Username { get; }
    public bool EmailVerified { get; }

    public FirebaseUserModel(string id, string email, string username, bool emailVerified)
    {
        Id = id;
        Email = email;
        Username = username;
        EmailVerified = emailVerified;
    }
}
