using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Desafio.Infra.Settings;
using System.Diagnostics.CodeAnalysis;

namespace DesafioPrimeiro.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class CORSExtensions
    {
        public static void UseCORS(this IApplicationBuilder app, IConfiguration configuration)
        {
            var corsSettings = configuration.LoadSettings<CORSSettings>("CORSSettings") ?? new CORSSettings().Default();

            app.UseCors(builder => builder
                                    .WithOrigins(corsSettings.Origins)
                                    .WithMethods(corsSettings.Methods)
                                    .WithHeaders(corsSettings.Headers)
                                    .Build());
        }
    }
}
