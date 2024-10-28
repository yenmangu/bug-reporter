using System;
using System.Text.Encodings.Web;
using FirebaseAuthenticationWrapper;
using FirebaseAuthenticationWrapper.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FirebaseAuthenticationWrapper.Tests;

[TestClass]
public class FirebaseAuthHandler_Test
{
    private Mock<IOptionsMonitor<AuthenticationSchemeOptions>> _mockOptions;
    private Mock<ILoggerFactory> _mockLoggerFactoy;
    private Mock<UrlEncoder> _mockEncoder;
    private Mock<TimeProvider> _mockClock;
    private FirebaseAuthHandler_extensionClass _mockHandler;
    private FirebaseAuthFunctionHandler functionHandler;

    [TestInitialize]
    public void SetupMocks()
    {
        _mockOptions = new Mock<IOptionsMonitor<AuthenticationSchemeOptions>>();
        _mockOptions
            .Setup(x => x.Get(It.IsAny<string>()))
            .Returns(new AuthenticationSchemeOptions());

        var logger = new Mock<ILogger<FirebaseAuthHandler>>();
        _mockLoggerFactoy = new Mock<ILoggerFactory>();
        _mockLoggerFactoy.Setup(x => x.CreateLogger(It.IsAny<String>())).Returns(logger.Object);

        _mockEncoder = new Mock<UrlEncoder>();
        _mockClock = new Mock<TimeProvider>();

        _mockHandler = new FirebaseAuthHandler_extensionClass(
            _mockOptions.Object,
            _mockLoggerFactoy.Object,
            _mockEncoder.Object,
            _mockClock.Object,
            functionHandler
        );
    }

    [TestMethod]
    public async Task TestFirebaseAuthHandler_NoAuthorizationHeader_ReturnsAuthenticate_ResultFail()
    {
        var result = await _mockHandler.HandleAuthenticateAsync_Test();

        Assert.AreEqual(AuthenticateResult.NoResult(), result);
    }
}

// Sub Class for testing

public class FirebaseAuthHandler_extensionClass : FirebaseAuthHandler
{
    public FirebaseAuthHandler_extensionClass(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        TimeProvider clock,
        FirebaseAuthFunctionHandler functionHandler
    )
        : base(options, logger, encoder, clock, functionHandler) { }

    public Task<AuthenticateResult> HandleAuthenticateAsync_Test() => HandleAuthenticateAsync();
}
