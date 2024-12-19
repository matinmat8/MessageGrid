# nullable disable

namespace BMO.API.Core.Entities.Requests;

public class AccountStatementsNotifierRequest
{
    public string Username { get; set; }
    public string Recipient { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public List<string> TradeList { get; set; }
}