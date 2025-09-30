using TourApp.Application.Interfaces;
using TourApp.Application.Service;
using TourApp.Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace TourApp.Application.DepenedencyInjection;

public static class BookingServiceDependencyInjection
{
  public static IServiceCollection AddBookingService(this IServiceCollection @this)
  {
    @this.AddScoped<IBookingService, BookingService>();

    return @this;
  }

  public static IServiceCollection AddDatabaseConnection(this IServiceCollection @this, string uri, string username, string password, int portNr)
  {
    //NOTE: this could have some setup that descides what type of database connection to use.
    //      In our case, we just use it for RabbitMQ.
    @this.AddRabbitMQRepository(uri, username, password, portNr);

    return @this;
  }
}
