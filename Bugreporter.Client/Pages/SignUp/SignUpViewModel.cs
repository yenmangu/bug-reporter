using Bugreporter.Client.Features;
using Bugreporter.Client.Shared;

namespace Bugreporter.Client.Pages.SignUp;

public class SignUpViewModel : ViewModelBase
{
    public SignUpFormViewModel SignUpFormViewModel { get; }

    public SignUpViewModel(SignUpFormViewModel signUpFormViewModel)
    {
        SignUpFormViewModel = signUpFormViewModel;
    }
}