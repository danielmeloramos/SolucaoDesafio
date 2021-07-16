using AutoMapper;
using NddCargo.Integration.Application.Tests.Initializer;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace NddCargo.Integration.Application.Tests.AutoMapperConfigurations
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Application.AutoMapper")]
    public class AutoMapperTest : TestBase
    {
        [Test]
        public void AutoMapper_DevePossuirUmaConfiguracaoDeMapeamentoValido() => Mapper.AssertConfigurationIsValid();
    }
}
