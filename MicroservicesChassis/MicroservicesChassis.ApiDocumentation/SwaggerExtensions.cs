using MicroservicesChassis.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace MicroservicesChassis.ApiDocumentation
{
    public static class SwaggerExtensions
    {
        private static readonly string SectionName = "swagger";

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            SwaggerOptions options;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<SwaggerOptions>(configuration.GetSection(SectionName));
                options = configuration.GetOptions<SwaggerOptions>(SectionName);
            }

            if (!options.Enabled)
                return services;

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(options.Name, new Info { Title = options.Title, Version = options.Version });
            });

            return services;
        }

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder)
        {
            var options = builder.ApplicationServices.GetService<IOptions<SwaggerOptions>>().Value;
            if (!options.Enabled)
                return builder;

            var routePrefix = string.IsNullOrWhiteSpace(options.RoutePrefix) ? "swagger" : options.RoutePrefix;
            builder.UseStaticFiles().UseSwagger(c => c.RouteTemplate = routePrefix + "/{documentName}/swagger.json");

            return builder.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint($"/{routePrefix}/{options.Name}/swagger.json", options.Title);
                x.RoutePrefix = routePrefix;
            });
        }
    }
}