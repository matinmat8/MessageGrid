using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Utilities;

namespace BMO.API.Core.Services;

public class OfflineOrderNotifier : INotifier<OfflineOrderNotifierRequest>
{

    private readonly IMessageSenderFactory _messageFactory;

    public OfflineOrderNotifier(IMessageSenderFactory messageFactory)
    {
        _messageFactory = messageFactory;
    }

    public async Task SetMessage(OfflineOrderNotifierRequest offlineOrderNotifierRequest)
    {
        var parameters = PropertyExtractor.ExtractProperties(offlineOrderNotifierRequest);
        parameters.Add(new KeyValuePair<string, object>("MessageType", MessageType.OfflineOrder));


        var smsSender = _messageFactory.CreateMessageSender(MessageTransportType.Sms);
        await smsSender.Send(parameters.ToArray());
    }
}