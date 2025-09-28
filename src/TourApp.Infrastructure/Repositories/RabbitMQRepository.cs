using System.Text;
using System.Text.Json;
using TourApp.Common.Models;
using TourApp.Infrastructure.Interfaces;
using RabbitMQ.Client;

namespace TourApp.Infrastructure.Repositories;

internal class RabbitMQRepository : IBookingRepository
{
  private readonly IConnectionFactory _connectionFacotry;
  private const string _EXCHANGE_NAME = "tour.topic";

  public RabbitMQRepository(IConnectionFactory connectionFacotry)
  {
    _connectionFacotry = connectionFacotry;
  }

  public async Task<bool> PlaceBookingAsync(CreateBooking booking, CancellationToken cancellationToken = default)
  {
    const string ROUTE_KEY = "tour.booked";
    try
    {
      using IConnection connection = await _connectionFacotry.CreateConnectionAsync(cancellationToken: cancellationToken);
      using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

      await channel.ExchangeDeclareAsync(_EXCHANGE_NAME, type: ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: cancellationToken);

      await SetupQueueOnExchangeAsync(channel, "tour.booked", ROUTE_KEY, cancellationToken);

      string message = JsonSerializer.Serialize(booking);
      byte[] messageBody = Encoding.UTF8.GetBytes(message);

      BasicProperties properties = new()
      {
        ContentType = "application/json",
        CorrelationId = Guid.NewGuid().ToString("D"),
      };

      await channel.BasicPublishAsync("tour.topic", ROUTE_KEY, mandatory: true, properties, messageBody, cancellationToken: cancellationToken);
      return true;
    }
    catch (Exception e)
    {
      Console.WriteLine(e.Message);
      return false;
    }
  }

  public async Task<bool> CancelBookingAsync(CancelBooking cancellation, CancellationToken cancellationToken = default)
  {
    const string ROUTE_KEY = "tour.cancelled";
    try
    {
      using IConnection connection = await _connectionFacotry.CreateConnectionAsync(cancellationToken: cancellationToken);
      using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

      await channel.ExchangeDeclareAsync(_EXCHANGE_NAME, type: ExchangeType.Topic, cancellationToken: cancellationToken);

      await SetupQueueOnExchangeAsync(channel, "tour.canclled", ROUTE_KEY, cancellationToken);

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

  public async Task<bool> SetupRepositoryAsync(CancellationToken cancellationToken = default)
  {
    using IConnection connection = await _connectionFacotry.CreateConnectionAsync(cancellationToken: cancellationToken);
    using IChannel channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

    await channel.ExchangeDeclareAsync(_EXCHANGE_NAME, type: ExchangeType.Topic, durable: true, autoDelete: false, cancellationToken: cancellationToken);
    return true;
  }

  private async Task SetupQueueOnExchangeAsync(IChannel channel, string queueName, string routeKey, CancellationToken cancellationToken)
  {
    var queue = await channel.QueueDeclareAsync(
      queueName,
      durable: true,
      exclusive: false,
      autoDelete: false,
      cancellationToken: cancellationToken
    );
    await channel.QueueBindAsync(queueName, _EXCHANGE_NAME, routeKey);
  }
}
