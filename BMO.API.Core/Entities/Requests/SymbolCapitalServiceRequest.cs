# nullable disable

namespace BMO.API.Core.Entities.Requests;

public class SymbolCapitalServiceRequest
{
    public string Username { get; set; }
    public string Recipient { get; set; }
    public string Symbol { get; set; }
    public string IncreaseDate { get; set; }
    public string CompanyName { get; set; }
    public string IncreasePercentage { get; set; }
    public string SourceOfCapital { get; set; }
}