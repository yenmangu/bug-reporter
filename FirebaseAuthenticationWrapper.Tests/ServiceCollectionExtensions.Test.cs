using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using FirebaseAuthenticationWrapper;
using FirebaseAuthenticationWrapper.Extensions;
using FirebaseAuthenticationWrapper.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FirebaseAuthenticationWrapper.Tests;

[TestClass]
public class ServiceCollectionExtensionTest
{
    private ServiceCollection _serviceCollection;
    private IServiceProvider _serviceProvider;
    private FirebaseAuthHandler _firebaseAuthHandler;

    [TestInitialize]
    public void Setup()
    {
        _serviceCollection = new ServiceCollection();
    }

    // Add helper method to Initialise FirebaseAuthenticationService
    private void AddFirebaseAuthenticationService()
    {
        _serviceCollection.AddFirebaseAuthenticationService();
        _serviceCollection.AddLogging();
        _serviceProvider = _serviceCollection.BuildServiceProvider();
        _firebaseAuthHandler = _serviceProvider.GetRequiredService<FirebaseAuthHandler>();
    }

    [TestMethod]
    public void AddFirebaseAuthenticationService_Should_Be_Extension_Method()
    {
        AddFirebaseAuthenticationService();
        AssertServiceTypeT_registered<IAuthenticationSchemeProvider>("Should register service <T>");
    }

    private void AssertServiceTypeT_registered<TService>(string message)
    {
        var authService = _serviceProvider.GetService<TService>();
        Assert.IsNotNull(authService, message);
    }

    [TestMethod]
    public void AddFirebaseAuthentication_ShouldRegisterAuthScheme()
    {
        AddFirebaseAuthenticationService();
        var authService = _serviceProvider.GetService<IAuthenticationSchemeProvider>();

        Assert.IsNotNull(_serviceProvider, "Auth scheme should be added");
        Assert.IsNotNull(authService, "Should register auth service");
    }

    [TestMethod]
    public void AddFirebaseAuthentication_ShouldRegisterFirebaseAuthHandler()
    {
        AddFirebaseAuthenticationService();

        Assert.IsNotNull(_firebaseAuthHandler, "Should add FirebaseAuthHandler");
    }
}
