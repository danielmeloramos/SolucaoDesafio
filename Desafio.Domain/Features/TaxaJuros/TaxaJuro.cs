using System;

namespace Desafio.Domain.Features.TaxaJuros
{
    public class TaxaJuro
    {
        public decimal Valor => 0.01M;

        public string CalcularJuros(decimal valorInicial, int meses, decimal juros)
        {
            var valorFinal = Math.Pow(Convert.ToDouble(valorInicial * (1 + juros)), meses);
            var valorTruncado = Math.Truncate(100 * valorFinal) / 100;
            return valorTruncado.ToString();
        }
    }
}
