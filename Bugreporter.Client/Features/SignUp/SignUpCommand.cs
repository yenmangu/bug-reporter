using Bugreporter.Client.Shared.Commands;
using Firebase.Auth;

namespace Bugreporter.Client.Features.SignUp;

public class SignUpCommand : AsyncCommandBase
{
    private readonly SignUpFormViewModel _viewModel;
    private readonly FirebaseAuthClient _authClient;

    public SignUpCommand(SignUpFormViewModel signUpFormViewModel, FirebaseAuthClient authClient)
    {
        _viewModel = signUpFormViewModel;
        _authClient = authClient;
    }

    protected override async Task ExecuteAsync(object parameter)
    {
        if (_viewModel.Password != _viewModel.ConfirmPassword)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error", "Passwords do not match", "OK"
            );
            return;
        }
        try
        {
            await _authClient.CreateUserWithEmailAndPasswordAsync(
                _viewModel.Email, _viewModel.Password
            );
            await Application.Current.MainPage.DisplayAlert("Success", "User Created", "OK");
            await Shell.Current.GoToAsync("//SignIn");
        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error, failed to sign up please try again later", e.Message, "OK"
            );
        }
    }
}