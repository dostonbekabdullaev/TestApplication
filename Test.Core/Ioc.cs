namespace Test.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;

public static class Ioc
{
    public static void AddLogger(this WebApplicationBuilder builder)
    {
        // NLog: Setup NLog for Dependency injection
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
        builder.Services.AddTransient<Logger.ILogger, Logger.Logger>(q => new Logger.Logger("*"));
    }
}
