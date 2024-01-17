using RabbitMQ.Client;
using System;
using System.Text;

public class RabbitMQService : IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMQService(string hostName, int port, string userName, string password)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = port,
            UserName = userName,
            Password = password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void SendMessage(string queueName, string message)
    {
        _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
    }

    public void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();
    }
}
