using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Dehasoft.DataAccess.Models;
using Dehasoft.DataAccess.Repositories;
using Dehasoft.Business.Services;
using Dehasoft.Business.Mappings;

public static class ServiceExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        var connStr = config.GetConnectionString("DefaultConnection");
        services.AddSingleton(new DatabaseContext(connStr!));

        var api = config.GetSection("ApiSettings").Get<ApiSettings>();
        services.AddSingleton(new ApiService(api!.ApiKey, api.ApiSecret));

        services.AddAutoMapper(typeof(MappingProfile));

        services.AddScoped<IOrderRepository,OrderRepository>();
        services.AddScoped<IProductRepository,ProductRepository>();
        services.AddScoped<IOrderService,OrderService>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}
