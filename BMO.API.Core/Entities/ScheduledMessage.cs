using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BMO.API.Core.Entities.Enums;
using Newtonsoft.Json;

// # nullable disable

namespace BMO.API.Core.Entities
{
    [Table("ScheduledMessage")]
    public class ScheduledMessage
    {
        [Key][Column("Id")] public int Id { get; set; }
        [Required][Column("userName")] public string UserName { get; set; }
        [Required][Column("recipient")] public string Recipient { get; set; }
        [Column("messageTransportType")] public MessageTransportType MessageTransportType { get; set; }
        [Column("messageType")] public MessageType MessageType { get; set; }
        [Column("scheduledTime")] public DateTime ScheduledTime { get; set; }
        [Column("messageContent")] public string? MessageContent { get; set; } // Serialized dictionary
        [Column("status")] public MessageStatus MessageStatus { get; set; }



        // Helper property to work with the dictionary
        [NotMapped]
        public Dictionary<string, object>? Variables
        {
            get => string.IsNullOrEmpty(MessageContent)
                ? new Dictionary<string, object>()
                : JsonConvert.DeserializeObject<Dictionary<string, object>>(MessageContent);
            set => MessageContent = JsonConvert.SerializeObject(value);
        }
    }
}
