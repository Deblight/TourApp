using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace TourApp.Admin.Console;

public static class Program
{
  public static async Task Main(string[] argv)
  {
    var factory = new ConnectionFactory()
    {
      HostName = "localhost",
      Port = 52001,
      UserName = "user",
      Password = "password"
    };

    using var connection = await factory.CreateConnectionAsync();
    using var channel = await connection.CreateChannelAsync();

    await channel.ExchangeDeclareAsync("tour.topic", type: ExchangeType.Topic, durable: true, autoDelete: false);
    await channel.QueueDeclareAsync("tour.backoffice", durable: true, exclusive: false, autoDelete: false);
    await channel.QueueBindAsync("tour.backoffice", "tour.topic", "tour.*");

    var consumer = new AsyncEventingBasicConsumer(channel);

    consumer.ReceivedAsync += async (model, ea) =>
    {
      var message = Encoding.UTF8.GetString(ea.Body.ToArray());

      System.Console.WriteLine(message);
      await channel.BasicAckAsync(ea.DeliveryTag, false);
    };

    await channel.BasicConsumeAsync("tour.backoffice", autoAck: false, consumer: consumer);

    System.Console.ReadKey();
  }
}
