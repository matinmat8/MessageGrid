# nullable disable

namespace BMO.API.Core.Entities.Requests;

public class AccountChargeRequest
{
    public string Username { get; set; }
    public string CreditAmount { get; set; }
    public string CreditDate { get; set; }
    public string PaymentMethod { get; set; }
}