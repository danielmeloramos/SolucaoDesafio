using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;
using DesafioPrimeiro.Api.Features.Publics;
using DesafioPrimeiro.Api.Tests.Initializer;

namespace DesafioPrimeiro.Api.Tests.Features.Publics
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    [Category("Api.Publics.Controller")]
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

