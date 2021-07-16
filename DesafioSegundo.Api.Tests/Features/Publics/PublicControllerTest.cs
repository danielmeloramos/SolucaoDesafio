using FluentAssertions;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using DesafioSegundo.Api.Features.Publics;
using Microsoft.AspNetCore.Mvc;
using DesafioSegundo.Api.Tests.Initializer;

namespace DesafioSegundo.Api.Tests.Features.Publics
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("ApiSegundo.Publics.Controller")]
    public class PublicControllerTest : TestBase
    {
        private PublicController _controller;

        [SetUp]
        public void Initialize()
        {
            _controller = new PublicController()
            {
                ControllerContext = GetControllerContext()
            };
        }

        [Test]
        public void Test_AccountControllerTest_DeveriaRealizarVerificacaoStatusApi()
        {
            // Action
            var callback = _controller.IsAlive();

            // Assert
            var result = callback.Should().BeOfType<OkObjectResult>().Subject.Value;
            result.Should().BeOfType<bool>();
        }
    }
}

