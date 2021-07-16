using Newtonsoft.Json;

namespace DesafioSegundo.Api.Features.TaxaJuros.ViewModels
{
    /// <summary>
    /// Representa o valor da taxa de juros
    /// </summary>
    public class TaxaJuroCalcularViewModel
    {
        /// <summary>
        /// Valor
        /// </summary>
        [JsonProperty("valor")]
        public string Valor { get; set; }
    }
}
