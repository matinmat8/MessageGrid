using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Utilities;

namespace BMO.API.Core.Services;

public class AccountChargeNotifier : INotifier<AccountChargeRequest>
{
    private readonly IMessageSenderFactory _messageSenderFactory;

    public AccountChargeNotifier(IMessageSenderFactory messageSender)
    {
        _messageSenderFactory = messageSender;
    }

    public async Task SetMessage(AccountChargeRequest accountChargeRequest)
    {

        var parameters = PropertyExtractor.ExtractProperties(accountChargeRequest);
        parameters.Add(new KeyValuePair<string, object>("MessageType", MessageType.AccountCreditCharge));

        var smsSender = _messageSenderFactory.CreateMessageSender(MessageTransportType.Sms);
        await smsSender.Send(parameters.ToArray());
    }
}