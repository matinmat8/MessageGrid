using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Utilities;

namespace BMO.API.Core.Services;

public class BrokerGiftCreditNotifier : INotifier<BrokerGiftCreditRequest>
{


    private readonly IMessageSenderFactory _messageFactory;

    public BrokerGiftCreditNotifier(IMessageSenderFactory messageFactory)
    {
        _messageFactory = messageFactory;
    }

    public async Task SetMessage(BrokerGiftCreditRequest brokerGiftCreditRequest)
    {
        var parameters = PropertyExtractor.ExtractProperties(brokerGiftCreditRequest);
        parameters.Add(new KeyValuePair<string, object>("MessageType", MessageType.BrokerGiftCredit));

        var smsSender = _messageFactory.CreateMessageSender(MessageTransportType.Sms);
        await smsSender.Send(parameters.ToArray());
    }
}