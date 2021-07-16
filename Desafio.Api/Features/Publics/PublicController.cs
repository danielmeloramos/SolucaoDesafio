using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DesafioPrimeiro.Api.Exceptions;

namespace DesafioPrimeiro.Api.Features.Publics
{
    /// <summary>
    /// Controlador
    /// </summary>
    [Route("api/public")]
    [ApiExplorerSettings(GroupName = "Public - Serviços relacionados a conectividade")]
    public class PublicController : ControllerBase
    {
        /// <summary>
        /// Solicitar status da API
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
        [Route("is-alive")]
        [AllowAnonymous]
        public IActionResult IsAlive() => Ok(true);
    }
}