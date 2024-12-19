using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Utilities;

namespace BMO.API.Core.Services;

public class SymbolCapitalNotifier : INotifier<SymbolCapitalServiceRequest>
{

    private readonly IMessageSenderFactory _messageFactory;

    public SymbolCapitalNotifier(IMessageSenderFactory messageFactory)
    {
        _messageFactory = messageFactory;
    }

    public async Task SetMessage(SymbolCapitalServiceRequest symbolCapitalServiceRequest)
    {
        var parameters = PropertyExtractor.ExtractProperties(symbolCapitalServiceRequest);
        parameters.Add(new KeyValuePair<string, object>("MessageType", MessageType.SymbolCapitalIncrease));

        var smsSender = _messageFactory.CreateMessageSender(MessageTransportType.Sms);
        await smsSender.Send(parameters.ToArray());
    }

}