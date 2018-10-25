using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace MicroservicesChassis.Logging
{
    public static class LoggingExtensions
    {
        private static readonly string SectionName = "serilog";

        public static IWebHostBuilder UseLogging(this IWebHostBuilder webHostBuilder, string applicationName)
        {
            webHostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                loggerConfiguration.Enrich.FromLogContext()
                    .MinimumLevel.Verbose()
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .Enrich.WithProperty("ApplicationName", applicationName);

                var serilogOptions = context.Configuration.GetSection(SectionName).Get<SerilogOptions>();
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

            loggerConfiguration.WriteTo.Logger(l => l.Filter.ByIncludingOnly(x => x.Level == level).WriteTo.Console());
        }

        private static void ConfigureFileOutput(LoggerConfiguration loggerConfiguration, LoggingTarget loggingTarget)
        {
            if (loggingTarget == null || !loggingTarget.Enabled)
                return;

            if (!Enum.TryParse<LogEventLevel>(loggingTarget.Level, true, out var level))
                level = LogEventLevel.Debug;

            loggerConfiguration.WriteTo.Logger(l => l.Filter.ByIncludingOnly(x => x.Level == level).WriteTo.File(@"Logs\logs.txt", rollingInterval: RollingInterval.Day));
        }
    }
}
