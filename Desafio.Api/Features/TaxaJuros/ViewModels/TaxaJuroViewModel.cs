using Newtonsoft.Json;

namespace DesafioPrimeiro.Api.Features.TaxaJuros.ViewModels
{
    /// <summary>
    /// Representa o valor da taxa de juros
    /// </summary>
    public class TaxaJuroViewModel
    {
        /// <summary>
        /// Valor
        /// </summary>
        [JsonProperty("valor")]
        public decimal Valor { get; set; }
    }
}
