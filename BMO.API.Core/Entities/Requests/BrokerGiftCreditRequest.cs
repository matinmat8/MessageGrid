
# nullable disable
namespace BMO.API.Core.Entities.Requests;

public class BrokerGiftCreditRequest
{
    public string Username { get; set; }
    public string Recipient { get; set; }
    public string CreditAmount { get; set; }
    public string CreditDate { get; set; }
    public string ReasonForGift { get; set; }
}