using System.Text;
using System.Text.Json;
using TourApp.Common.Models;
using TourApp.Infrastructure.Interfaces;
using RabbitMQ.Client;

namespace TourApp.Infrastructure.Repositories;

internal class RabbitMQRepository : IBookingRepository
{
  private readonly IConnectionFactory _connectionFacotry;
  private const string _EXCHANGE_NAME = "tour_exchange";
  // private const string _QUEUE_NAME = "tours";

  public RabbitMQRepository(IConnectionFactory connectionFacotry)
  {
    _connectionFacotry = connectionFacotry;
  }

  public async Task<bool> PlaceBookingAsync(CreateBooking booking, CancellationToken cancellationToken = default)
  {
    try
    {
      using IConnection connection = await _connectionFacotry.CreateConnectionAsync(cancellationToken: cancellationToken);
      using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

      await channel.ExchangeDeclareAsync(_EXCHANGE_NAME, type: ExchangeType.Fanout, cancellationToken: cancellationToken);

      string message = JsonSerializer.Serialize(booking);
      byte[] messageBody = Encoding.UTF8.GetBytes(message);

      BasicProperties properties = new()
      {
        ContentType = "application/json",
        CorrelationId = Guid.NewGuid().ToString("D"),
      };

      const string ROUTE_KEY = "tour.booked";
      await channel.BasicPublishAsync(_EXCHANGE_NAME, ROUTE_KEY, mandatory: true, properties, messageBody, cancellationToken: cancellationToken);
      return true;
    }
    catch (Exception)
    {
      //TODO: lets do some logging
      return false;
    }
  }

  public async Task<bool> CancelBookingAsync(CancelBooking cancellation, CancellationToken cancellationToken = default)
  {
    try
    {
      using IConnection connection = await _connectionFacotry.CreateConnectionAsync(cancellationToken: cancellationToken);
      using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

      await channel.ExchangeDeclareAsync(_EXCHANGE_NAME, type: ExchangeType.Fanout, cancellationToken: cancellationToken);

      string message = JsonSerializer.Serialize(cancellation);
      byte[] messageBody = Encoding.UTF8.GetBytes(message);

      BasicProperties properties = new()
      {
        ContentType = "application/json",
        CorrelationId = Guid.NewGuid().ToString("D"),
      };

      const string ROUTE_KEY = "tour.cancelled";
      await channel.BasicPublishAsync(_EXCHANGE_NAME, ROUTE_KEY, mandatory: true, properties, messageBody, cancellationToken: cancellationToken);
      return true;
    }
    catch (Exception)
    {
      //TODO: lets do some logging
      return false;
    }
  }
}
