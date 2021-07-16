using AutoMapper;
using DesafioSegundo.Api.Features.TaxaJuros.ViewModels;

namespace DesafioSegundo.Api.Features.TaxaJuros
{
    /// <summary>
    /// Realiza mapeamento entre ViewModels e Domains
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Construtor
        /// </summary>
        public MappingProfile()
        {
            CreateMap<string, TaxaJuroCalcularViewModel>()
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src));
        }
    }
}
