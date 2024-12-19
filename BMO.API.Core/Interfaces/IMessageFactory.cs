using BMO.API.Core.Entities.Enums;

namespace BMO.API.Core.Interfaces
{
    public interface IMessageSenderFactory 
    {
        IMessageSender CreateMessageSender(MessageTransportType messageTransportType);
    }
}