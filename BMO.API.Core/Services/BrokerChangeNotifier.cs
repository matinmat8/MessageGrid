using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Utilities;

namespace BMO.API.Core.Services;

public class BrokerChangeNotifier : INotifier<BrokerChangeNotifierRequest>
{

    private readonly IMessageSenderFactory _messageFactory;

    public BrokerChangeNotifier(IMessageSenderFactory messageFactory)
    {
        _messageFactory = messageFactory;
    }

    public async Task SetMessage(BrokerChangeNotifierRequest brokerChangeNotifierRequest)
    {
        var parameters = PropertyExtractor.ExtractProperties(brokerChangeNotifierRequest);
        parameters.Add(new KeyValuePair<string, object>("MessageType", MessageType.ChangeBroker));

        var smsSender = _messageFactory.CreateMessageSender(MessageTransportType.Sms);
        await smsSender.Send(parameters.ToArray());
    }
}