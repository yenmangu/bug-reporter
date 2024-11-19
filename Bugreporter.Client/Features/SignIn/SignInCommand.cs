using Bugreporter.Client.Shared.Commands;
using Firebase.Auth;

namespace Bugreporter.Client.Features.SignIn;

public class SignInCommand : AsyncCommandBase
{
    private readonly SignInFormViewModel _viewModel;
    private readonly FirebaseAuthClient _authClient;

    public SignInCommand(SignInFormViewModel viewModel, FirebaseAuthClient authClient)
    {
        _authClient = authClient;
        _viewModel = viewModel;
    }

    protected override async Task ExecuteAsync(object parameter)
    {
        try
        {
            UserCredential? x = await _authClient.SignInWithEmailAndPasswordAsync(
                _viewModel.Email, _viewModel.Password
            );
            await Application.Current.MainPage.DisplayAlert(
                "Success!", "You have been logged in.", "OK"
            );

            await Shell.Current.GoToAsync("//ReportBug");
        }
        catch (Exception e)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error, failed to sign in", e.Message, "OK"
            );
        }
    }
}