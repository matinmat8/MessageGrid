namespace BMO.API.Core.Configuration;
// public class SmsProvidersSettings
// {
//     public SmsProviderSettings PrimaryProvider { get; set; }
//     public SmsProviderSettings SecondaryProvider { get; set; }
// }

#nullable disable

public class PatternCodes
{
    public string AccountStatement { get; set; }
    public string TradeConfirmation { get; set; }
    public string AccountAlert { get; set; }
    public string MarketUpdate { get; set; }
    public string TaxDocument { get; set; }
    public string RegulatoryNotice { get; set; }
    public string PromotionalOffer { get; set; }
    public string SecurityAlert { get; set; }
    public string CustomerServiceCommunication { get; set; }
    public string EducationalContent { get; set; }
    public string DailyTradeSummary { get; set; }
    public string IPOAnnouncement { get; set; }
    public string UnusedCapitalProfit { get; set; }
    public string MiscellaneousNotification { get; set; }
    public string UserInformation { get; set; }
    public string UserInformationChange { get; set; }
    public string BirthdayAndEventGreetings { get; set; }
    public string CapitalIncrease { get; set; }
    public string DividendAllocation { get; set; }
    public string TradingBan { get; set; }
    public string SpecificStockTradingBan { get; set; }
    public string AccountCreditAmount { get; set; }
    public string CreditReceived { get; set; }
    public string FutureEvents { get; set; }
    public string InternationalMarketAlert { get; set; }
    public string TechnicalAndFundamentalAnalysis { get; set; }
    public string ImportantEconomicAndPoliticalNews { get; set; }
    public string SystematicAlerts { get; set; }
    public string SpecialOffers { get; set; }
    public string InformationSms { get; set; }
    public string Default { get; set; }
}


public class SmsProviderSettings
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FromRecipient { get; set; }
    public string ApiKey { get; set; }
    public string ApiUrl { get; set; }
    // public Dictionary<string, string> PatternCodes { get; set; }
    public PatternCodes PatternCodes { get; set; }
}
