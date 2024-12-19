# nullable disable

namespace BMO.API.Core.Entities.Requests;

public class UserInformationNotifierRequest
{
    public string Username { get; set; }
    public string Recipient { get; set; }
    public List<string> ChangeDetails { get; set; }
}