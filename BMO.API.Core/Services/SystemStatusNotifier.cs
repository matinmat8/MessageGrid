using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Utilities;

namespace BMO.API.Core.Services;

public class SystemStatusNotifier : INotifier<SystemStatusNotifierRequest>
{

    private readonly IMessageSenderFactory _messageFactory;

    public SystemStatusNotifier(IMessageSenderFactory messageFactory)
    {
        _messageFactory = messageFactory;
    }

    public async Task SetMessage(SystemStatusNotifierRequest systemStatusNotifierRequest)
    {
        var parameters = PropertyExtractor.ExtractProperties(systemStatusNotifierRequest);
        parameters.Add(new KeyValuePair<string, object>("MessageType", MessageType.SystemStatus));


        var smsSender = _messageFactory.CreateMessageSender(MessageTransportType.Sms);
        await smsSender.Send(parameters.ToArray());
    }
}