using Confluent.Kafka;
using Microsoft.Extensions.Options;
using BMO.API.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Services;

public class CapitalIncreaseConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly KafkaSettings _kafkaSettings;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CapitalIncreaseConsumer(IOptions<KafkaSettings> kafkaSettings, IServiceScopeFactory serviceScopeFactory)
    {
        _kafkaSettings = kafkaSettings.Value;
        _serviceScopeFactory = serviceScopeFactory;

        var config = new ConsumerConfig
        {
            GroupId = _kafkaSettings.Consumer.GroupId,
            BootstrapServers = $"{_kafkaSettings.Consumer.Server}:{_kafkaSettings.Consumer.Port}",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("capital-increase-topic");

        return Task.Run(async () => {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    var jsonObject = JsonConvert.DeserializeObject<SymbolCapitalServiceRequest>(consumeResult.Message.Value);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var capitalIncreaseService = scope.ServiceProvider.GetRequiredService<SymbolCapitalNotifier>();
                        await capitalIncreaseService.SetMessage(jsonObject!);
                    }
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
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _consumer.Close();
        await base.StopAsync(stoppingToken);
    }
}
