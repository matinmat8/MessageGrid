# nullable disable

namespace BMO.API.Core.Entities.Requests;

public class BrokerChangeNotifierRequest
{
    public string Username { get; set; }
    public string Recipient { get; set; }
    public string Symbol { get; set; }
    public string NewBrokerName { get; set; }
    public string ChangeDate { get; set; }
}