using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public class MessageFactoryProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<MessageTransportType, Type> _factories;

    public MessageFactoryProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _factories = new Dictionary<MessageTransportType, Type>
        {
            { MessageTransportType.Sms, typeof(IMessageSender) },
            { MessageTransportType.Email, typeof(IMessageSender) },
            { MessageTransportType.ScheduledSms, typeof(IMessageSender) }
        };
    }

    public IMessageSenderFactory GetFactory(MessageTransportType transportType)
    {
        if (_factories.TryGetValue(transportType, out var factoryType))
        {
            return (IMessageSenderFactory)_serviceProvider.GetRequiredService(factoryType);
        }
        throw new ArgumentException("Invalid message transport type");
    }
}
