namespace FirebaseAuthenticationWrapper.Common;

public interface IFirebaseToken
{
    IReadOnlyDictionary<string, object> Claims { get; }
}
