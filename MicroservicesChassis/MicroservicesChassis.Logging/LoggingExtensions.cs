using System;
using MicroservicesChassis.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace MicroservicesChassis.Logging
{
    public static class LoggingExtensions
    {
        private static readonly string SectionName = "serilog";

        public static IWebHostBuilder UseLogging(this IWebHostBuilder webHostBuilder, string applicationName = null)
        {
            webHostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                var appIdentityOptions = context.Configuration.GetAppIdentityOptions();
                var appName = applicationName ?? appIdentityOptions.Name;
                loggerConfiguration.Enrich.FromLogContext()
                    .MinimumLevel.Verbose()
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("ApplicationName", appName);

                var serilogOptions = context.Configuration.GetOptions<SerilogOptions>(SectionName);
                ConfigureConsoleOutput(loggerConfiguration, serilogOptions.Console);
                ConfigureFileOutput(loggerConfiguration, serilogOptions.File);
            });
            return webHostBuilder;
        }

        private static void ConfigureConsoleOutput(LoggerConfiguration loggerConfiguration, LoggingTarget loggingTarget)
        {
            if(loggingTarget == null || !loggingTarget.Enabled)
                return;

            if (!Enum.TryParse<LogEventLevel>(loggingTarget.Level, true, out var level))
                level = LogEventLevel.Information;

            loggerConfiguration.WriteTo.Logger(l => l.MinimumLevel.Is(level).WriteTo.Console());
        }

        private static void ConfigureFileOutput(LoggerConfiguration loggerConfiguration, LoggingTarget loggingTarget)
        {
            if (loggingTarget == null || !loggingTarget.Enabled)
                return;

            if (!Enum.TryParse<LogEventLevel>(loggingTarget.Level, true, out var level))
                level = LogEventLevel.Debug;

            loggerConfiguration.WriteTo.Logger(l => l.MinimumLevel.Is(level).WriteTo.File(@"Logs\logs.txt", rollingInterval: RollingInterval.Day));
        }
    }
}
