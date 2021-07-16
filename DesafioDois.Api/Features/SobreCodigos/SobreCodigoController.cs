using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DesafioSegundo.Api.Exceptions;
using Microsoft.Extensions.Logging;

namespace DesafioSegundo.Api.Features.SobreCodigos
{
    /// <summary>
    /// Controlador
    /// </summary>
    [Route("api/show-me-the-code")]
    [ApiExplorerSettings(GroupName = "Public - Serviços relacionados ao fonte no Github")]
    public class SobreCodigoController : ControllerBase
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Usamos IoC aqui para injetar a instância única (singleton) nesse controller
        /// </summary>
        public SobreCodigoController(ILogger<SobreCodigoController> logger) : base()
        {
            _logger = logger;
        }

        /// <summary>
        /// Solicitar a url de onde encontra-se o fonte no Github
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">NotFound</response>
        /// <response code="500">InternalServerError</response>
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ExceptionPayload), 400)]
        [ProducesResponseType(typeof(ExceptionPayload), 401)]
        [ProducesResponseType(typeof(ExceptionPayload), 404)]
        [ProducesResponseType(typeof(ExceptionPayload), 500)]
        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IActionResult Get() => Ok("https://github.com/danielmeloramos/SolucaoDesafio");
    }
}