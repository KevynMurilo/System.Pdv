using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Services.Printer;

namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class ThermalDependencyInjection
{
    public static IServiceCollection AddThermalDependencies(this IServiceCollection services)
    {
        services.AddScoped<IThermalPrinterService, ThermalPrinterService>();
        return services;
    }
}
