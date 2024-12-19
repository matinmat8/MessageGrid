# nullable disable

namespace BMO.API.Core.Entities.Requests;

public class OtpCodeSenderRequest
{
    public string Username { get; set; }
    public string Recipient { get; set; }
    public string OtpCode { get; set; }
    public string ValidityPeriod { get; set; }
}