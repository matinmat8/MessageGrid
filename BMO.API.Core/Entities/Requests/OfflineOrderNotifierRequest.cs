# nullable disable

namespace BMO.API.Core.Entities.Requests;

public class OfflineOrderNotifierRequest
{
    public string Username { get; set; }
    public string Recipient { get; set; }
    public string OrderCode { get; set; }
    public string OfflineOrderStatus { get; set; }
    public string Symbol { get; set; }
    public string Quantity { get; set; }
    public string Price { get; set; }
}