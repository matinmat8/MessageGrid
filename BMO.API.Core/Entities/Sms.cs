using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BMO.API.Core.Entities.Enums;
using Newtonsoft.Json;

namespace BMO.API.Core.Entities
{
    [Table("SmsMessage")]
    public class SmsMessage
    {
        [Key][Column("Id")] public int Id { get; set; }
        [Column("userName")] public  string UserName { get; set; }
        [Required][MaxLength(11)][Column("recipient")] public string Recipient { get; set; }
        [Required][Column("messageContent")] public string MessageContent { get; set; } // Serialized dictionary
        [Column("sentTime")] public DateTime? SentTime { get; set; }
        [Required][Column("messageStatus")] public MessageStatus MessageStatus { get; set; }
        [Column("messageType")] public MessageType MessageType { get; set; }
        [Column("patternCode")] public string? PatternCode { get; set; }
        [Column("tries")] public int Tries { get; set; }
        [Column("errorMessage")] public string? ErrorMessage { get; set; }



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
