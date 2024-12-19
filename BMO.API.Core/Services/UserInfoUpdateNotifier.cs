using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;

namespace BMO.API.Core.Services;

public class UserInformationNotifier : INotifier<UserInformationNotifierRequest>
{

    private readonly IMessageSenderFactory _messageFactory;


    public UserInformationNotifier(IMessageSenderFactory messageFactory)
    {
        _messageFactory = messageFactory;
    }

    public Task SetMessage(UserInformationNotifierRequest userInformationNotifierRequest)
    {
        throw new NotImplementedException();
    }
}