namespace BMO.API.Core.Interfaces
{
    public interface IMessageSender 
    {
        Task Send(params KeyValuePair<string,object>[] parameters);
    }
}