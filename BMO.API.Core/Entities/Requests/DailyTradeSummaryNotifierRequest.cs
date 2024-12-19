#nullable disable

namespace BMO.API.Core.Entities.Requests;

public class DailyTradeSummaryNotifierRequest
{
    public string Username { get; set; }
    public string Recipient { get; set; }
    public string TradeList { get; set; } // trade of days
    public string Inventory { get; set; }
}