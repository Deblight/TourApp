using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TourApp.Interfaces;

namespace TourApp.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventApiDemo(this IServiceCollection services)
        => services.AddSingleton<IEventApi, DemoEventApi>();

    public static IServiceCollection AddEventApiHttp(this IServiceCollection services, IConfiguration cfg)
    {
        var baseUrl = cfg.GetSection("Api")["BaseUrl"] ?? "https://localhost:5001/";
        services.AddHttpClient<IEventApi, HttpEventApi>(c => c.BaseAddress = new Uri(baseUrl));
        return services;
    }
}
