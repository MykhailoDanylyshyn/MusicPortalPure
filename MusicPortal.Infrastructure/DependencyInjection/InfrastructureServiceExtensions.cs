using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MusicPortal.Application.Interfaces;
using MusicPortal.Infrastructure.Persistence;
using MusicPortal.Infrastructure.Repositories;

namespace MusicPortal.Infrastructure.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MusicPortalContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, EFUnitOfWork>();
        return services;
    }
}
