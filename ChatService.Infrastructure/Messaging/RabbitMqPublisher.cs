using ChatService.Domain.Entities;
using ChatService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChatService.Infrastructure.Messaging;

public class RabbitMqPublisher : IEventPublisher
{
    private readonly IConfiguration _configuration;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task PublishAsync(Message message, CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory() 
        { 
            HostName = _configuration["RabbitMq:Host"],
            UserName = _configuration["RabbitMq:Username"],
            Password = _configuration["RabbitMq:Password"]
        };

        
        using var connection = await factory.CreateConnectionAsync(cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        var exchangeName = "chat_events";
        await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Fanout, cancellationToken: cancellationToken);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(exchange: exchangeName, routingKey: "", body: body, cancellationToken: cancellationToken);
    }
}
