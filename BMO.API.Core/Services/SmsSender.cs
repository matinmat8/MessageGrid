using System.Text;
using BMO.API.Core.Configuration;
using BMO.API.Core.Entities;
using BMO.API.Core.Entities.Enums;
using BMO.API.Core.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BMO.API.Core.Services;

public class SmsSender : IMessageSender
{
    private readonly SmsProviderSettings _smsProviderSettings;
    private readonly HttpClient _httpClient;
    private readonly IRepository<SmsMessage> _repository;

    private const int RecipientLength = 11;

    public SmsSender(IOptions<SmsProviderSettings> smsProviderSettings, HttpClient httpClient, IRepository<SmsMessage> repository)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _smsProviderSettings = smsProviderSettings?.Value ?? throw new ArgumentNullException(nameof(smsProviderSettings));
        _repository = repository;
    }

    public async Task Send(params KeyValuePair<string, object>[] parameters)
    {
        var (recipient, messageType, variables) = ExtractParameters(parameters);
        ValidateParameters(recipient, messageType);

        var patternCode = GetPatternCode(messageType);

        await SendSmsAsync(recipient, patternCode, messageType, variables);
    }


    // Get pattern code based on message type
    private string GetPatternCode(MessageType messageType)
    {
        var patternCodeProperty = _smsProviderSettings.PatternCodes?.GetType().GetProperty(messageType.ToString());
        return patternCodeProperty != null
            ? patternCodeProperty.GetValue(_smsProviderSettings.PatternCodes)?.ToString() ?? _smsProviderSettings.PatternCodes.Default
            : _smsProviderSettings.PatternCodes?.Default;
    }

    private (string recipient, MessageType messageType, Dictionary<string, object> variables) ExtractParameters(IEnumerable<KeyValuePair<string, object>> parameters)
    {
        var recipient = string.Empty;
        var messageType = MessageType.None;
        var variables = new Dictionary<string, object>();

        foreach (var (key, value) in parameters)
        {
            switch (key)
            {
                case "Recipient":
                    recipient = value.ToString()!;
                    break;
                case "MessageType":
                    Enum.TryParse(value.ToString(), out messageType);
                    break;
                case "MessageContent":
                    DeserializeMessageContent(value.ToString()!, variables);
                    break;
                default:
                    variables[key] = value.ToString()!;
                    break;
            }
        }

        return (recipient, messageType, variables);
    }

    private void DeserializeMessageContent(string messageContentStr, Dictionary<string, object> variables)
    {
        var messageContentDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(messageContentStr);
        if (messageContentDict != null)
        {
            foreach (var nestedParam in messageContentDict)
            {
                variables[nestedParam.Key] = nestedParam.Value.ToString() ?? "";
            }
        }
    }

    private void ValidateParameters(string recipient, MessageType messageType)
    {
        if (string.IsNullOrEmpty(recipient))
        {
            throw new InvalidOperationException("The recipient number is required.");
        }

        if (recipient.Length != RecipientLength)
        {
            throw new InvalidOperationException($"The recipient number must be {RecipientLength} digits long.");
        }

        if (messageType == MessageType.None)
        {
            throw new InvalidOperationException("The MessageType is required.");
        }
    }

    private async Task SendSmsAsync(string recipient, string patternCode, MessageType messageType, Dictionary<string, object> variables)
    {
        var payload = new
        {
            code = patternCode,
            sender = _smsProviderSettings.FromRecipient,
            recipient = recipient,
            variable = variables
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, _smsProviderSettings.ApiUrl)
        {
            Content = content
        };

        request.Headers.Add("apikey", _smsProviderSettings.ApiKey);

        variables.TryGetValue("UserName", out var username);
        var extractedUsername = username!.ToString();

        try
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            await HandleResponse(extractedUsername, recipient, variables, patternCode, messageType, response, true);
        }
        catch (HttpRequestException httpEx)
        {
            await HandleException(extractedUsername!, recipient, variables, patternCode, messageType, httpEx.Message, "HTTP Request Error");
        }
        catch (Exception ex)
        {
            await HandleException(extractedUsername!, recipient, variables, patternCode, messageType, ex.Message, "General Error");
        }
    }

    private async Task HandleResponse(string extractedUsername, string recipient, Dictionary<string, object> variables, string patternCode, MessageType messageType, HttpResponseMessage response, bool success)
    {
        string responseFromServer = await response.Content.ReadAsStringAsync();

        var smsMessage = new SmsMessage
        {
            UserName = extractedUsername!,
            Recipient = recipient,
            MessageContent = JsonConvert.SerializeObject(variables),
            SentTime = DateTime.UtcNow,
            MessageStatus = success ? MessageStatus.Delivered : MessageStatus.Failed,
            MessageType = messageType,
            PatternCode = patternCode,
            Tries = 1,
            ErrorMessage = success ? null : "Error sending SMS"
        };

        await _repository.AddAsync(smsMessage);

        Console.WriteLine(responseFromServer);
        System.Diagnostics.Debug.WriteLine(responseFromServer);
    }

    private async Task HandleException(string extractedUsername, string recipient, Dictionary<string, object> variables, string patternCode, MessageType messageType, string errorMessage, string errorType)
    {
        var fullErrorMessage = $"{errorType}: {errorMessage}";

        var smsMessage = new SmsMessage
        {
            UserName = extractedUsername!,
            Recipient = recipient,
            MessageContent = JsonConvert.SerializeObject(variables),
            SentTime = DateTime.UtcNow,
            MessageStatus = MessageStatus.Failed,
            MessageType = messageType,
            PatternCode = patternCode,
            Tries = 1,
            ErrorMessage = fullErrorMessage
        };

        await _repository.AddAsync(smsMessage);

        Console.WriteLine(fullErrorMessage);
        System.Diagnostics.Debug.WriteLine(fullErrorMessage);
    }
}