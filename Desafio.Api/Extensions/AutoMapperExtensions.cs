using Microsoft.Extensions.DependencyInjection;
using Desafio.Application;
using Desafio.Infra.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace DesafioPrimeiro.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class AutoMapperExtensions
    {
        public static void AddAutoMapper(this IServiceCollection services) => services.ConfigureProfiles(typeof(Api.Startup), typeof(AppModule));
    }
}