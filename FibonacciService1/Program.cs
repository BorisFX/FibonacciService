using FibonacciService1.Requests;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "fib_queue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

var consumer = new EventingBasicConsumer(channel);


consumer.Received += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var numberString = Encoding.UTF8.GetString(body);

    if (!long.TryParse(numberString, out long n))
    {
        Console.WriteLine($"Ошибка: Невозможно преобразовать число {numberString} в тип long.");
        return; 
    }

    Console.WriteLine(n);

    if (n > long.MaxValue || n < 0)
    {
        Console.WriteLine($"Ошибка: Число {n} не может быть обработано.");
        return;
    }

    var result = Fib(n);
    Console.WriteLine(result);

    var requestModel = new CalculateFibonacciStepRequest
    {
        FibonacciNumber = result 
    };

    string jsonRequest = JsonSerializer.Serialize(requestModel);

    using var restClient = new HttpClient();

    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
    var response = await restClient.PostAsync("http://localhost:5020/api/fib/next-step", content);

    if (response.IsSuccessStatusCode)
    {
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseContent);
    }
    else
    {
        Console.WriteLine($"Error: {response.StatusCode}");
    }
};
channel.BasicConsume(queue: "fib_queue",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine("srv2 started consuming");
Thread.Sleep(Timeout.Infinite);

 static long Fib(long n)
{
    double a = n * (1 + Math.Sqrt(5)) / 2.0;
    return (long)Math.Round(a);
}
