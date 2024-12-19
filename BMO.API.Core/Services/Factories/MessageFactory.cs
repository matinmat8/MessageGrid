using BMO.API.Core.Configuration;
using BMO.API.Core.Entities;
using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Services;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


public class MessageFactory : IMessageSenderFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IRepository<SmsMessage> _repository;

    public MessageFactory(IServiceProvider serviceProvider, IRepository<SmsMessage> repository)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _repository = repository;

    }

    public IMessageSender CreateMessageSender(MessageTransportType messageTransportType)
    {
        // When you call CreateMessageSender in order to create a Sender based on transport type
        // Checking the health of MessageProvider and choose the healthy provider

        if (messageTransportType == MessageTransportType.Sms)
        {
            var PrimaryServiceProvider = CreatePrimaryProvider();
            if (true)
            {
                return PrimaryServiceProvider;
            }
        }
        else if (messageTransportType == MessageTransportType.Email)
        {
            return new EmailSender();
        }

        return new EmailSender();

        // var SecondaryServiceProvider = CreateSecondaryProvider();
        // if ( SecondaryServiceProvider.IsHealthy())
        // {
        // return SecondaryServiceProvider;
        // }

    }


    private SmsSender CreatePrimaryProvider()
    {
        // var settings = _serviceProvider.GetRequiredService<IOptions<SmsSender>>();
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient();
        var smsProviderSettings = _serviceProvider.GetRequiredService<IOptions<SmsProviderSettings>>();
        return new SmsSender(smsProviderSettings, httpClient, _repository);
    }

    private SmsSender CreateSecondaryProvider()
    {
        var smsProviderSettings = _serviceProvider.GetRequiredService<IOptions<SmsProviderSettings>>();
        var httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient();
        // return new SecondarySmsSender(smsProviderSettings, httpClient);
        return new SmsSender(smsProviderSettings, httpClient, _repository);
    }
}
