using Dehasoft.Business.Mappings;
using Dehasoft.Business.Services;
using Dehasoft.DataAccess.Models;
using Dehasoft.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        var connStr = config.GetConnectionString("DefaultConnection");
        services.AddSingleton(new DatabaseContext(connStr!));

        services.AddScoped<ILogService, LogService>();

        services.AddSingleton<ApiService>(provider =>
        {
            var api = config.GetSection("ApiSettings").Get<ApiSettings>()!;
            var logService = provider.GetRequiredService<ILogService>();
            return new ApiService(api.ApiKey, api.ApiSecret, logService);
        });

        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
