using System.Collections.Concurrent;
using BMO.API.Core.Entities;
using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BMO.API.Core.Services
{
    public class ScheduledMessageBackgroundService : BackgroundService
    {
        private readonly ILogger<ScheduledMessageBackgroundService> _logger;
        private Timer _timer;
        private readonly ConcurrentQueue<ScheduledMessage> _scheduledMessages;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ScheduledMessageBackgroundService(ILogger<ScheduledMessageBackgroundService> logger, IServiceScopeFactory scopeFactory, ConcurrentQueue<ScheduledMessage> scheduledMessages)
        {
            _logger = logger;
            _serviceScopeFactory = scopeFactory;
            _scheduledMessages = scheduledMessages;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");
            _timer = new Timer(FetchMessagesFromDb, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));
            Task.Run(() => ProcessMessageQueue(cancellationToken), cancellationToken);
            return Task.CompletedTask;
        }

        private async void ProcessMessageQueue(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                if (!_scheduledMessages.TryDequeue(out var message)) continue;
                var smsSender = scope.ServiceProvider.GetRequiredService<IMessageSenderFactory>().CreateMessageSender(message.MessageTransportType);
                var messageContentDict = message.Variables;

                var messageDict = new Dictionary<string, object>
                {
                    { "UserName", message.UserName },
                    { "Recipient", message.Recipient },
                    { "MessageType", message.MessageType.ToString() },
                    { "MessageContent", JsonConvert.SerializeObject(messageContentDict) },
                };

                var keyValuePairArray = messageDict.Select(kv => new KeyValuePair<string, object>(kv.Key, kv.Value.ToString())).ToArray();

                await smsSender.Send(keyValuePairArray);
            }
        }

        private async void FetchMessagesFromDb(object? state)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _repository = scope.ServiceProvider.GetRequiredService<IRepository<ScheduledMessage>>();
                var statusSpec = new MessageStatusSpecification(MessageStatus.Scheduled);
                var messages = await _repository.GetBySpecificationAsync(statusSpec);

                if (messages != null)
                {
                    foreach (var message in messages)
                    {
                        // Checks if it's time to send the message.
                        if (message.ScheduledTime <= DateTime.Now && message.MessageStatus == MessageStatus.Scheduled)
                        {
                            _scheduledMessages.Enqueue(message);
                        }
                    }
                }
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
