# nullable disable

namespace BMO.API.Core.Entities.Requests;

public class CelebrationNotifierRequest
{
    public string Username { get; set; }
    public string Recipient { get; set; }
    public string OccasionName { get; set; }
}