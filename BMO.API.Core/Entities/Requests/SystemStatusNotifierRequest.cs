# nullable disable

namespace BMO.API.Core.Entities.Requests;

public class SystemStatusNotifierRequest
{
    public string Username { get; set; }
    // Probably it will be a list of Recipients
    public string Recipient { get; set; }
    public string IssueType { get; set; }
    public string EstimatedResolutionTime { get; set; }
}