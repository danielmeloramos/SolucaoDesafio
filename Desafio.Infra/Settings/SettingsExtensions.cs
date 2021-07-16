using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Desafio.Infra.Settings
{
    [ExcludeFromCodeCoverage]
    public static class SettingsExtensions
    {
        public static T LoadSettings<T>(this IConfiguration configuration, string sectionName, IServiceCollection service = null) where T : class
        {
            var sections = configuration.GetSection(sectionName);

            service?.Configure<T>(sections);

            return sections.Get<T>();
        }
    }
}
