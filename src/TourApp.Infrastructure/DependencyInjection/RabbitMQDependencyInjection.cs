using TourApp.Infrastructure.Interfaces;
using TourApp.Infrastructure.Repositories;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;

namespace TourApp.Infrastructure.DependencyInjection;

public static class RabbitMQDependencyInjection
{
  public static IServiceCollection AddRabbitMQRepository(this IServiceCollection @this, string rabbitMQUri, string username, string password)
  {
    @this.AddScoped<IBookingRepository, RabbitMQRepository>();
    @this.AddSingleton<IConnectionFactory, ConnectionFactory>(_ =>
    {
      ConnectionFactory factory = new()
      {
        HostName = rabbitMQUri,
        UserName = username,
        Password = password
      };

      return factory;
    });

    return @this;
  }
}
