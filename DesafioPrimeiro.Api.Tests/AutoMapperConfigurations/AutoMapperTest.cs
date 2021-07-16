using AutoMapper;
using DesafioPrimeiro.Api.Tests.Initializer;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace DesafioPrimeiro.Api.Tests.AutoMapperConfigurations
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Api.AutoMapper")]
    public class AutoMapperTest : TestBase
    {
        [Test]
        public void AutoMapper_DevePossuirUmaConfiguracaoDeMapeamentoValido() => Mapper.AssertConfigurationIsValid();
    }
}
