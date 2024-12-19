using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Utilities;

namespace BMO.API.Core.Services;

public class CelebrationNotifier : INotifier<CelebrationNotifierRequest>
{

    private readonly IMessageSenderFactory _messageFactory;


    public CelebrationNotifier(IMessageSenderFactory messageFactory)
    {
        _messageFactory = messageFactory;
    }

    public async Task SetMessage(CelebrationNotifierRequest celebrationNotifierRequest)
    {
        var parameters = PropertyExtractor.ExtractProperties(celebrationNotifierRequest);
        parameters.Add(new KeyValuePair<string, object>("MessageType", MessageType.BirthdayAndEventGreetings));

        var smsSender = _messageFactory.CreateMessageSender(MessageTransportType.Sms);
        await smsSender.Send(parameters.ToArray());
    }

    // If it is called from controller, it will log the message itself.
    public Task LogMessage(CelebrationNotifierRequest celebrationNotifier)
    {
        throw new NotImplementedException();
    }


}