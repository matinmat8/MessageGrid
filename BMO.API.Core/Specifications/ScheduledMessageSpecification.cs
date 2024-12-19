using System.Linq.Expressions;
using BMO.API.Core.Entities;
using BMO.API.Core.Entities.Enums;
using Core.Interfaces;

public class ScheduledMessageSpecification : ISpecification<ScheduledMessage, ScheduledMessage>
{
    public Expression<Func<ScheduledMessage, bool>> Criteria { get; }
    public List<Expression<Func<ScheduledMessage, object>>> Includes { get; } = new List<Expression<Func<ScheduledMessage, object>>>();
    public Expression<Func<ScheduledMessage, object>> OrderBy { get; }
    public Expression<Func<ScheduledMessage, ScheduledMessage>> Selector { get; }

    public ScheduledMessageSpecification(string username, string recipient, MessageStatus status, MessageType messageType)
    {
        Criteria = m => m.UserName == username && m.Recipient == recipient && m.MessageStatus == status && m.MessageType == messageType;
        Selector = m => m; // Return full entity
    }
}
