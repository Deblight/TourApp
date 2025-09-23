using System.Text;
using System.Text.Json;
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

  public async Task<bool> PlaceBookingAsync(object booking, CancellationToken cancellationToken = default)
  {
    try
    {
      using IConnection connection = await _connectionFacotry.CreateConnectionAsync(cancellationToken: cancellationToken);
      using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

      const string ROUTE_KEY = "tour.booked";
      await EnsureQueueExistsAsync(channel, ROUTE_KEY, cancellationToken);

      string message = JsonSerializer.Serialize(booking);
      byte[] messageBody = Encoding.UTF8.GetBytes(message);

      BasicProperties properties = new()
      {
        ContentType = "application/json",
        CorrelationId = Guid.NewGuid().ToString("D"),
      };

      await channel.BasicPublishAsync(_EXCHANGE_NAME, ROUTE_KEY, mandatory: true, properties, messageBody, cancellationToken: cancellationToken);
      return true;
    }
    catch (Exception)
    {
      //TODO: lets do some logging
      return false;
    }
  }

  public async Task<bool> CancelBookingAsync(object cancellation, CancellationToken cancellationToken = default)
  {
    try
    {
      using IConnection connection = await _connectionFacotry.CreateConnectionAsync(cancellationToken: cancellationToken);
      using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

      const string ROUTE_KEY = "tour.cancelled";
      await EnsureQueueExistsAsync(channel, ROUTE_KEY, cancellationToken);

      string message = JsonSerializer.Serialize(cancellation);
      byte[] messageBody = Encoding.UTF8.GetBytes(message);

      BasicProperties properties = new()
      {
        ContentType = "application/json",
        CorrelationId = Guid.NewGuid().ToString("D"),
      };

      await channel.BasicPublishAsync(_EXCHANGE_NAME, ROUTE_KEY, mandatory: true, properties, messageBody, cancellationToken: cancellationToken);
      return true;
    }
    catch (Exception)
    {
      //TODO: lets do some logging
      return false;
    }
  }

  private async Task EnsureQueueExistsAsync(IChannel channel, string routeKey, CancellationToken cancellationToken)
  {
    await channel.ExchangeDeclareAsync(_EXCHANGE_NAME, type: ExchangeType.Topic, cancellationToken: cancellationToken);
    var queue = await channel.QueueDeclareAsync(routeKey, durable: true, autoDelete: false, cancellationToken: cancellationToken);

    await channel.QueueBindAsync(queue.QueueName, _EXCHANGE_NAME, routeKey, cancellationToken: cancellationToken);
  }
}
