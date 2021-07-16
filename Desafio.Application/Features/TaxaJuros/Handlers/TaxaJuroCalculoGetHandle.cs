using Desafio.Application.Features.TaxaJuros.Queries;
using Desafio.Domain.Features.TaxaJuros;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Desafio.Application.Features.TaxaJuros.Handlers
{
    public class TaxaJuroCalculoGetHandle : IRequestHandler<TaxaJuroCalculoGetQuery, string>
    {
        public async Task<string> Handle(TaxaJuroCalculoGetQuery request, CancellationToken cancellationToken)
        {
            var taxaDeJuro = new TaxaJuro();
            var resposta = taxaDeJuro.CalcularJuros(request.ValorInicial, request.Meses, 0.01M);
            return await Task.FromResult(resposta);
        }
    }
}
