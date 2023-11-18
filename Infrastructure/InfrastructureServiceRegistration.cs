using Application.Persistence;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddDbContext<CustomerContext>(option =>
           option.UseSqlServer(configuration.GetConnectionString("CustomerConnectionString")));

        return services;

    }
}