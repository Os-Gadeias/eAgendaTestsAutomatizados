using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace eAgenda.Infra.Compartilhado.Logging;

public static class SerilogLoggerFactory
{
    public static void AddSerilogLogger(
        this IServiceCollection services,
        IConfiguration configuration,
        ILoggingBuilder logging,
        IWebHostEnvironment environment
    )
    {
        Log.Logger = SerilogFactory.Create(configuration, environment);

        // Remove o provedor padrão de logs da Microsoft
        logging.ClearProviders();

        services.AddSerilog(Log.Logger);
    }
}
