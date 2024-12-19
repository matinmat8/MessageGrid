using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;

namespace BMO.API.Core.Services;

public class AccountStatementsNotifier : INotifier<AccountStatementsNotifierRequest>
{

    private readonly IMessageSenderFactory _messageFactory;


    public AccountStatementsNotifier(IMessageSenderFactory messageFactory)
    {
        _messageFactory = messageFactory;
    }

    public Task SetMessage(AccountStatementsNotifierRequest accountStatementsNotifierRequest)
    {
        throw new NotImplementedException();
    }
}