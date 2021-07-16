using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace DesafioPrimeiro.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtensions
    {
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Desafio Primeiro API",
                    Description = "Api de Desafio Técnico",
                    Contact = new OpenApiContact
                    {
                        Name = "Desafio",
                        Email = string.Empty
                    }
                });

                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                        return new[] { api.GroupName };
                    
                    var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null)
                        return new[] { controllerActionDescriptor.ControllerName };
                    
                    throw new InvalidOperationException("Incapaz de determinar tag para endpoint.");
                });

                c.DocInclusionPredicate((name, api) => true);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void ConfigSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Desafio V1");
                c.InjectStylesheet($"{swaggerJsonBasePath}/swagger-ui/custom.css");
            });

            app.UseStaticFiles();
        }
    }
}
