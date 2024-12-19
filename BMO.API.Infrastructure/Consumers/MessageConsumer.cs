using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using BMO.API.Core.Configuration;
using BMO.API.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System.Linq;
using BMO.API.Core.Entities.Enums;

public class MessageConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly MessageFactoryProvider _messageFactoryProvider;
    private readonly KafkaSettings _kafkaSettings;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MessageConsumer(IOptions<KafkaSettings> kafkaSettings, MessageFactoryProvider messageFactoryProvider, IServiceScopeFactory serviceScopeFactory)
    {
        _kafkaSettings = kafkaSettings.Value;
        _messageFactoryProvider = messageFactoryProvider;
        _serviceScopeFactory = serviceScopeFactory;

        var config = new ConsumerConfig
        {
            GroupId = _kafkaSettings.Consumer.GroupId,
            BootstrapServers = $"{_kafkaSettings.Consumer.Server}:{_kafkaSettings.Consumer.Port}",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.Run(async () =>
    {
        _consumer.Subscribe("sms-topic");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var messageTransportType = MessageTransportTypeExtractor.ExtractMessageTransportType(consumeResult.Message.Value);
                // var parameters = ParameterExtractor.ExtractParameters(consumeResult.Message.Value).ToList();

                var factory = _messageFactoryProvider.GetFactory(messageTransportType);
                var sender = factory.CreateMessageSender(MessageTransportType.Sms);
                // await sender.Send(parameters.ToArray());
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
        }
        catch (Exception)
        {
            _consumer.Close();
            // Log or handle the exception as needed
        }
    }, stoppingToken);

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _consumer.Close();
        await base.StopAsync(stoppingToken);
    }
}
