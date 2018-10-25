using Microsoft.Extensions.Configuration;

namespace MicroservicesChassis.Common
{
    public static class ConfigurationExtensions
    {
        private static readonly string AppIdentitySection = "appIdentity";

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string section) where TModel : new()
        {
            return configuration.GetSection(section).Get<TModel>();
        }

        public static AppIdentityOptions GetAppIdentityOptions(this IConfiguration configuration)
        {
            return configuration.GetSection(AppIdentitySection).Get<AppIdentityOptions>();
        }
    }
}
