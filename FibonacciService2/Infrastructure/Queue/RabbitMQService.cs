using FibonacciService2.Exceptions;
using FibonacciService2.Infrastructure.Settings;
using RabbitMQ.Client;
using System.Text;

namespace FibonacciService2.Infrastructure.Queue
{
    public class RabbitMQService(AppSettings options) : IMessageQueueService
    {
        private readonly AppSettings _options = options;

        public void SendToFibQueue(long result)
        {
            try
            {

                var factory = new ConnectionFactory() { HostName = _options.RabbitMQSettings.HostName };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.QueueDeclare(queue: _options.RabbitMQSettings.QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(result.ToString());
                channel.BasicPublish(exchange: "",
                                     routingKey: _options.RabbitMQSettings.QueueName,
                                     basicProperties: null,
                                     body: body);
            }
            catch (Exception ex)
            {
                throw new FibonacciService2Exception($"Ошибка при отправке числа в очередь: {ex.Message}", ex);
            }
        }
    }
}
