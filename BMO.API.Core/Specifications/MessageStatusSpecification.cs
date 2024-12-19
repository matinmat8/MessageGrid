using System.Linq.Expressions;
using BMO.API.Core.Entities;
using BMO.API.Core.Entities.Enums;
using Core.Interfaces;

namespace BMO.API.Core.Specifications;
public class MessageStatusSpecification : ISpecification<ScheduledMessage, ScheduledMessage>
{
    public Expression<Func<ScheduledMessage, bool>> Criteria { get; }
    public List<Expression<Func<ScheduledMessage, object>>> Includes { get; } = new List<Expression<Func<ScheduledMessage, object>>>();
    public Expression<Func<ScheduledMessage, object>> OrderBy { get; }
    public Expression<Func<ScheduledMessage, ScheduledMessage>> Selector { get; }

    public MessageStatusSpecification(MessageStatus status)
    {
        Criteria = m => m.MessageStatus == status;
        Selector = m => m; // Return full entity
    }
}
