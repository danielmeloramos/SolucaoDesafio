using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DesafioPrimeiro.Api.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Desafio.Domain.Features.TaxaJuros;
using AutoMapper;
using Desafio.Application.Features.TaxaJuros.Queries;
using System.Threading.Tasks;
using DesafioPrimeiro.Api.Features.TaxaJuros.ViewModels;

namespace DesafioPrimeiro.Api.Features.TaxaJuros
{
    /// <summary>
    /// Controlador
    /// </summary>
    [Route("api/taxa-juros")]
    [ApiExplorerSettings(GroupName = "Public - Serviços relacionados a taxa de juros")]
    public class TaxaJuroController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        /// <summary>
        /// Usamos IoC aqui para injetar a instância única (singleton) nesse controller
        /// </summary>
        public TaxaJuroController(IMediator mediator, ILogger<TaxaJuroController> logger) : base()
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Solicitar taxa de juros
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
        public async Task<IActionResult> Get()
        {
            return Ok(Mapper.Map<TaxaJuro, TaxaJuroViewModel>(await _mediator.Send(new TaxaJuroGetQuery())));
        }
    }
}