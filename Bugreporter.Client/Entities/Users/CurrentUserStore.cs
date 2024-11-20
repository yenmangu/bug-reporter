using Firebase.Auth;

namespace Bugreporter.Client.Entities.Users;

public class CurrentUserStore
{
    public User CurrentUser { get; set; }
}