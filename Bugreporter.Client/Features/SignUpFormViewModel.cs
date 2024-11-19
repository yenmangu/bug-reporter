using System.Windows.Input;
using Bugreporter.Client.Features.SignUp;
using Bugreporter.Client.Shared;
using Firebase.Auth;

namespace Bugreporter.Client.Features;

public class SignUpFormViewModel : ViewModelBase
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

    private string _confirmPassword;

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged(nameof(ConfirmPassword));
        }
    }

    public ICommand SignUpCommand { get; }

    public SignUpFormViewModel(FirebaseAuthClient authClient)
    {
        SignUpCommand = new SignUpCommand(this, authClient);
    }
}