using BMO.API.Core.Entities;
using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Utilities;
using Core.Interfaces;

namespace BMO.API.Core.Services;

public class DailyTradeSummaryNotifier : INotifier<DailyTradeSummaryNotifierRequest>
{

    private readonly IMessageSenderFactory _messageFactory;
    private readonly IRepository<ScheduledMessage> _repository;

    public DailyTradeSummaryNotifier(IMessageSenderFactory messageFactory, IRepository<ScheduledMessage> repository)
    {
        _messageFactory = messageFactory;
        _repository = repository;
    }

    public async Task SetMessage(DailyTradeSummaryNotifierRequest dailyTradeSummaryNotifierRequest)
    {
        var parameters = PropertyExtractor.ExtractProperties(dailyTradeSummaryNotifierRequest);
        parameters.Add(new KeyValuePair<string, object>("MessageType", MessageType.DailyTradeSummary));

        // Try to fetch scheduled message with same username and recipient
        var spec = new ScheduledMessageSpecification(dailyTradeSummaryNotifierRequest.Username, dailyTradeSummaryNotifierRequest.Recipient, MessageStatus.Scheduled, MessageType.DailyTradeSummary);
        var existingMessages = await _repository.GetBySpecificationAsync(spec);


        if (existingMessages.Any())
        {

            // Update the MessageContent of ScheduledMessage

            var existingMessage = existingMessages.First();
            var existingVariables = existingMessage.Variables;

            var existingTradeList = existingVariables != null && existingVariables.ContainsKey("TradeList") ? existingVariables["TradeList"].ToString() : string.Empty;
            var combinedTradeList = existingTradeList + "\n" + dailyTradeSummaryNotifierRequest.TradeList;

            if (existingVariables != null)
            {
                existingVariables["TradeList"] = combinedTradeList;
                existingVariables["Inventory"] = dailyTradeSummaryNotifierRequest.Inventory;

                existingMessage.Variables = existingVariables;
            }

            existingMessage.ScheduledTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17, 0, 0);

            _repository.Update(existingMessage);
        }

        else // there is no scheduled message with provided UserName and Recipient
        {
            var newVariables = new Dictionary<string, object> {
                { "TradeList", dailyTradeSummaryNotifierRequest.TradeList },
                { "Inventory", dailyTradeSummaryNotifierRequest.Inventory }
            };
            if (dailyTradeSummaryNotifierRequest.Recipient.Length < 11)
            {
                throw new CustomException(1);
            }
            // Create a new scheduled message
            var scheduledMessage = new ScheduledMessage
            {
                UserName = dailyTradeSummaryNotifierRequest.Username,
                Recipient = dailyTradeSummaryNotifierRequest.Recipient,
                MessageTransportType = MessageTransportType.Sms,
                MessageType = MessageType.DailyTradeSummary,
                ScheduledTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 17, 0, 0),
                MessageStatus = MessageStatus.Scheduled,
                Variables = newVariables
            };

            await _repository.AddAsync(scheduledMessage);

        }
        try
        {
            await _repository.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error", ex);
        }

    }
}

// Update the existing message
// foreach (var message in existingMessages)
// {
//     var existingTradeData = message.Variables;
//     // var existingTradeData = JsonConvert.DeserializeObject<Dictionary<string, object>>(message.MessageContent);
//     foreach (var item in existingTradeData)
//     {
//         if (combinedTradeList.ContainsKey(item.Key))
//         {
//             combinedTradeList[item.Key] = item.Value;
//         }
//         else
//         {
//             combinedTradeList.Add(item.Key, item.Value);
//         }
//     }
// }
// var firstMessage = existingMessages.First();
// foreach (var duplicateMessage in existingMessages.Skip(1))
// {
//     _repository.Remove(duplicateMessage);
// }

// _repository.Update(existingMessage);