using BMO.API.Core.Configuration;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Services;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BMO.API.Infrastructure.Consumers;

public class OfflineOrderConsumer : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public OfflineOrderConsumer(IOptions<KafkaSettings> kafkaSettings, IServiceScopeFactory serviceScopeFactory)
    {
        var kafkaSettingsValue = kafkaSettings.Value;
        _serviceScopeFactory = serviceScopeFactory;

        var config = new ConsumerConfig
        {
            GroupId = kafkaSettingsValue.Consumer.GroupId,
            BootstrapServers = $"{kafkaSettingsValue.Consumer.Server}:{kafkaSettingsValue.Consumer.Port}",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe("Offline-Order-Topic");

        return Task.Run(async () =>
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumerResult = _consumer.Consume(stoppingToken);
                    var jsonObject = JsonConvert.DeserializeObject<OfflineOrderNotifierRequest>(consumerResult.Message.Value);

                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var offlineOrderNotifier = scope.ServiceProvider.GetRequiredService<OfflineOrderNotifier>();
                        await offlineOrderNotifier.SetMessage(jsonObject!);
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