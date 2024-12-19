namespace BMO.API.Core.Interfaces;

public interface INotifier<TRequest>
{
    /// <summary>
    /// Propare and Decide When and Whay type of message should have been sent
    /// </summary>
    /// <param name="request">A Request class instance</param>
    /// <returns></returns>
    Task SetMessage(TRequest request);
}