using System.Pdv.Core.Interfaces;
using System.Pdv.Infrastructure.Repositories;


namespace System.Pdv.Web.Configurations.DependencyInjection;

public static class TransactionDependencyInjection
{
    public static IServiceCollection AddTransactionDependencies(this IServiceCollection services)
    {
        services.AddScoped<ITransactionManager, TransactionManager>();
        return services;
    }
}
