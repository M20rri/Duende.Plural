// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Airlines Reservation!");

var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
string queueName = "Airline.booking.Queue";
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();


channel.QueueDeclare(queue: queueName,
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);


channel.QueueBind(queue: queueName,
                         exchange: "AIRLINES_BOOKING_EXCHANGE",
                         routingKey: "serve-ticket");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body;
    var message = Encoding.UTF8.GetString(body.ToArray());
    Console.WriteLine(message);
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
};

channel.BasicConsume(queue: queueName,
                     autoAck: false,
                     consumer: consumer);