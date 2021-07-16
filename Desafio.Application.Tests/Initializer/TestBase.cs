using NUnit.Framework;
using NddCargo.Integration.Infra.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.Initializer
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TestBase
    {
        [OneTimeSetUp]
        public void Setup() => this.ConfigureProfiles(typeof(AppModule));
    }
}
