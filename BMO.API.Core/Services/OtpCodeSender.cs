using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Utilities;

namespace BMO.API.Core.Services;

public class OtpCodeSender : INotifier<OtpCodeSenderRequest>
{

    private readonly IMessageSenderFactory _messageFactory;


    public OtpCodeSender(IMessageSenderFactory messageFactory)
    {
        _messageFactory = messageFactory;
    }

    public async Task SetMessage(OtpCodeSenderRequest otpCodeSenderRequest)
    {
        var parameters = PropertyExtractor.ExtractProperties(otpCodeSenderRequest);
        parameters.Add(new KeyValuePair<string, object>("MessageType", MessageType.OtpCode));

        var smsSender = _messageFactory.CreateMessageSender(MessageTransportType.Sms);
        await smsSender.Send(parameters.ToArray());
    }
}