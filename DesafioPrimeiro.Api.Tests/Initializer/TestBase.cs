using NUnit.Framework;
using Desafio.Infra.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Moq;
using System.Threading.Tasks;
using System;
using System.Diagnostics.CodeAnalysis;
using Desafio.Application;

namespace DesafioPrimeiro.Api.Tests.Initializer
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TestBase
    {
        [OneTimeSetUp]
        public void Setup() => this.ConfigureProfiles(typeof(Startup), typeof(AppModule));

        protected static HttpContext GetHttpContext()
        {
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            return new DefaultHttpContext
            {
                RequestServices = serviceProviderMock.Object,
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "desafio"),
                }, "mock"))
            };
        }

        protected ControllerContext GetControllerContext() => new()
        {
            HttpContext = GetHttpContext()
        };
    }
}
