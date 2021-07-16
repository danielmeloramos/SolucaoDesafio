using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Desafio.Application;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace DesafioPrimeiro.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class MvcExtensions
    {
        public static void AddMVC(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                //Isso é necessário para que o OData funcione no ASP.Net Core 3.1
                options.EnableEndpointRouting = false;
               // options.Filters.Add(typeof(ExceptionHandlerAttribute));
            })
            .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<AppModule>())
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.Formatting = Formatting.None;
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                opt.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });

            //Essa configuração faz com que o ASP.Net Core deixe executar automaticamente as validações
            //Assim dando a chance para que o ValidationPipeline execute-as.
            services.Configure<ApiBehaviorOptions>(config =>
            {
                config.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
