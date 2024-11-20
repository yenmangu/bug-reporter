using Bugreporter.Client.Entities.Users;
using Bugreporter.Client.Shared.Commands;
using Firebase.Auth;

namespace Bugreporter.Client.Features.SignIn;

public class SignInCommand : AsyncCommandBase
{
    private readonly SignInFormViewModel _viewModel;
    private readonly FirebaseAuthClient _authClient;
    private readonly CurrentUserStore _currentUserStore;

    public SignInCommand(SignInFormViewModel viewModel, FirebaseAuthClient authClient,
        CurrentUserStore currentUserStore)
    {
        _authClient = authClient;
        _viewModel = viewModel;
        _currentUserStore = currentUserStore;
    }

    protected override async Task ExecuteAsync(object parameter)
    {
        try
        {
            UserCredential userCredential = await _authClient.SignInWithEmailAndPasswordAsync(
                _viewModel.Email, _viewModel.Password
            );
            _currentUserStore.CurrentUser = userCredential.User;
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