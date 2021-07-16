using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Desafio.Application.Features.TaxaJuros.Queries
{
    public class TaxaJuroCalculoGetQuery : IRequest<string>
    {
        public TaxaJuroCalculoGetQuery(decimal valorInicial, int meses)
        {
            ValorInicial = valorInicial;
            Meses = meses;
        }

        public decimal ValorInicial { get; set; }
        
        public int Meses { get; set; }

        public ValidationResult Validate() => new TaxaJuroCalculoGetQueryValidator().Validate(this);
    }

    public class TaxaJuroCalculoGetQueryValidator : AbstractValidator<TaxaJuroCalculoGetQuery>
    {
        public TaxaJuroCalculoGetQueryValidator()
        {
            RuleFor(u => u.ValorInicial)
               .GreaterThan(0)
               .WithMessage("Valor inicial deve ser maior que 0.");

            RuleFor(u => u.Meses)
                .GreaterThan(0)
               .WithMessage("Meses deve ser maior que 0.");
        }
    }
}
