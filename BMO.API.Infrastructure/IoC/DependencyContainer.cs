using System.Collections.Concurrent;
using BMO.API.Core.Configuration;
using BMO.API.Core.Entities;
using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using BMO.API.Core.Services;
using BMO.API.Infrastructure.Consumers;
using BMO.API.Infrastructure.Repository;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BMO.API.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure settings
            services.Configure<SmsProviderSettings>(configuration.GetSection("SmsProviderSettings"));
            services.Configure<KafkaSettings>(configuration.GetSection("KafkaSetting"));

            // Register HttpClient with SmsSender
            services.AddHttpClient();

            // Register factories
            services.AddSingleton<MessageFactoryProvider>();
            services.AddTransient<IMessageSender, SmsSender>();
            services.AddTransient<IMessageSender, EmailSender>();

            // Register MessageSenderFactory
            services.AddScoped<IMessageSenderFactory, MessageFactory>();

            // Register MessageConsumer
            services.AddSingleton<MessageConsumer>();

            // Register Hosted Services
            services.AddHostedService<CapitalIncreaseConsumer>();
            services.AddHostedService<OfflineOrderConsumer>();
            services.AddHostedService<ScheduledMessageBackgroundService>();

            // services.AddScoped<IMessageSenderFactory, MessageFactory>();
            // services.AddScoped<IRepository<SmsMessage>, Repository<SmsMessage>>();



            services.AddScoped<IRepository<SmsMessage>, Repository<SmsMessage>>();
            services.AddScoped<IRepository<EmailMessage>, Repository<EmailMessage>>();
            services.AddScoped<IRepository<ScheduledMessage>, Repository<ScheduledMessage>>();
            // services.AddScoped<ISpecification<ScheduledMessage, ScheduledMessage>, ScheduledMessageSpecification>();


            services.AddSingleton(new ConcurrentQueue<ScheduledMessage>());


            // Register notifiers
            services.AddScoped<INotifier<DailyTradeSummaryNotifierRequest>, DailyTradeSummaryNotifier>();
            services.AddScoped<INotifier<SystemStatusNotifierRequest>, SystemStatusNotifier>();
            services.AddScoped<INotifier<AccountChargeRequest>, AccountChargeNotifier>();
            services.AddScoped<INotifier<BrokerChangeNotifierRequest>, BrokerChangeNotifier>();
            services.AddScoped<INotifier<CelebrationNotifierRequest>, CelebrationNotifier>();
            services.AddScoped<INotifier<BrokerGiftCreditRequest>, BrokerGiftCreditNotifier>();
            services.AddScoped<INotifier<OfflineOrderNotifierRequest>, OfflineOrderNotifier>();
            services.AddScoped<INotifier<OtpCodeSenderRequest>, OtpCodeSender>();
            services.AddScoped<INotifier<SymbolCapitalServiceRequest>, SymbolCapitalNotifier>();
            services.AddScoped<INotifier<UserInformationNotifierRequest>, UserInformationNotifier>();
        }
    }
}
