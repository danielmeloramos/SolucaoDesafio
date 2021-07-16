using AutoMapper;
using DesafioSegundo.Api.Tests.Initializer;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace DesafioSegundo.Api.Tests.AutoMapperConfigurations
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
