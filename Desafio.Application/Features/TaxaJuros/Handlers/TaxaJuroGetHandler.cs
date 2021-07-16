using Desafio.Application.Features.TaxaJuros.Queries;
using Desafio.Domain.Features.TaxaJuros;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Desafio.Application.Features.TaxaJuros.Handlers
{
    public class TaxaJuroGetHandler : IRequestHandler<TaxaJuroGetQuery, TaxaJuro>
    {
        public async Task<TaxaJuro> Handle(TaxaJuroGetQuery request, CancellationToken cancellationToken)
        {
            var taxaDeJuro = new TaxaJuro();
            return await Task.FromResult(taxaDeJuro);
        }
    }
}
