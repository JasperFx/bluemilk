using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BlueMilk.Microsoft.DependencyInjection
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseBlueMilk(this IWebHostBuilder builder)
        {
            return UseBlueMilk(builder, registry: null);
        }

        public static IWebHostBuilder UseBlueMilk(this IWebHostBuilder builder, ServiceRegistry registry)
        {
            return builder.ConfigureServices(services => { services.AddBlueMilk(registry); });
        }
        
        public static IServiceCollection AddBlueMilk(this IServiceCollection services)
        {
            return AddBlueMilk(services, registry: null);
        }

        public static IServiceCollection AddBlueMilk(this IServiceCollection services, 
            ServiceRegistry registry)
        
        {
            services.AddSingleton<IServiceProviderFactory<ServiceRegistry>, BlueMilkServiceProviderFactory>();
            services.AddSingleton<IServiceProviderFactory<IServiceCollection>, BlueMilkServiceProviderFactory>();
            
            return services;
        }
    }
}