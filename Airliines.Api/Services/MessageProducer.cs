using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Airlines.Api.Services
{
    public class MessageProducer : IMessageProducer
    {
        public Task<bool> SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "AIRLINES_BOOKING_EXCHANGE", type: ExchangeType.Direct);

            channel.QueueDeclare("Airline.booking.Queue", durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

            string messageSerialize = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageSerialize);

            string routingKey = "serve-ticket";

            channel.BasicPublish(exchange: "AIRLINES_BOOKING_EXCHANGE",
                                        routingKey: routingKey,
                                        basicProperties: null,
                                        body: body);

            return Task.FromResult(true);
        }
    }
}
