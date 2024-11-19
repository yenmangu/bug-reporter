using System;
using Bugreporter.Client.Features.SignIn;
using Bugreporter.Client.Shared;

namespace Bugreporter.Client.Pages.SignIn;

public class SignInViewModel : ViewModelBase
{
    public SignInFormViewModel SignInFormViewModel { get; }

    public SignInViewModel(SignInFormViewModel signInFormViewModel)
    {
        SignInFormViewModel = signInFormViewModel;
    }
}