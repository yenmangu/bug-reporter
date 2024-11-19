using System.Windows.Input;
using Bugreporter.Client.Shared;
using Firebase.Auth;

namespace Bugreporter.Client.Features.SignIn;

public class SignInFormViewModel : ViewModelBase
{
    private string _email;

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged(nameof(Email));
        }
    }

    private string _password;

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public ICommand SignInCommand { get; }

    public SignInFormViewModel(FirebaseAuthClient authClient)
    {
        SignInCommand = new SignInCommand(this, authClient);
    }
}