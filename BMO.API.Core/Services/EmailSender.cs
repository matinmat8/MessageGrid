using BMO.API.Core.Interfaces;

namespace BMO.API.Core.Services;

public class EmailSender: IMessageSender {

    public async Task Send(params KeyValuePair<string, object>[] parameters)
    {
        throw new NotImplementedException();
    }
}