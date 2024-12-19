// using System;
// using System.Text;
// using System.Collections.Generic;

// public class NotificationTemplates
// {
//     public string CreateDailyTradeSummaryMessage(string userName, DateTime tradeDate, List<Trade> trades)
//     {
//         var messageTemplate = @"
// موضوع: خلاصه معاملات امروز

// کاربر گرامی {userName}،

// خلاصه معاملات شما در تاریخ {tradeDate} به شرح زیر است:

// {tradesList}

// با تشکر،
// سامانه مدیریت بورس";

//         var tradesListBuilder = new StringBuilder();
//         foreach (var trade in trades)
//         {
//             var tradeLine = $"- {trade.TradeType}: {trade.Symbol}، تعداد: {trade.Quantity}، قیمت: {trade.PricePerShare} تومان (کل: {trade.TotalPrice} تومان)";
//             tradesListBuilder.AppendLine(tradeLine);
//         }

//         var tradesList = tradesListBuilder.ToString();

//         return messageTemplate
//             .Replace("{userName}", userName)
//             .Replace("{tradeDate}", tradeDate.ToString("yyyy/MM/dd"))
//             .Replace("{tradesList}", tradesList);
//     }

//     public string CreateInitialOfferingMessage(string userName, string companyName, string stockSymbol, DateTime offeringDate, decimal initialPrice, int sharesOffered)
//     {
//         var messageTemplate = @"
// موضوع: اطلاع‌رسانی پیرامون عرضه اولیه سهام

// کاربر گرامی {userName}،

// ما خوشحالیم به اطلاع شما برسانیم که عرضه اولیه سهام شرکت {companyName} در تاریخ {offeringDate} آغاز خواهد شد.

// جزئیات عرضه:
// - نام شرکت: {companyName}
// - نماد سهام: {stockSymbol}
// - تاریخ عرضه: {offeringDate}
// - قیمت اولیه: {initialPrice}
// - تعداد سهام قابل عرضه: {sharesOffered}

// لطفاً برای شرکت در این عرضه و کسب اطلاعات بیشتر به پنل کاربری خود مراجعه کنید.

// با تشکر،
// سامانه مدیریت بورس";

//         return messageTemplate
//             .Replace("{userName}", userName)
//             .Replace("{companyName}", companyName)
//             .Replace("{stockSymbol}", stockSymbol)
//             .Replace("{offeringDate}", offeringDate.ToString("yyyy/MM/dd"))
//             .Replace("{initialPrice}", initialPrice.ToString("N0"))
//             .Replace("{sharesOffered}", sharesOffered.ToString("N0"));
//     }

//     public string CreateUserChangeNotificationMessage(string userName, List<ChangeDetail> changeDetails)
//     {
//         var messageTemplate = @"
// موضوع: اطلاع‌رسانی پیرامون تغییرات اطلاعات کاربری

// کاربر گرامی {userName}،

// این پیام به اطلاع شما می‌رساند که تغییرات زیر در اطلاعات کاربری شما انجام شده است:

// {changeDetails}

// اگر شما این تغییرات را انجام نداده‌اید، لطفاً بلافاصله با پشتیبانی تماس بگیرید.

// با تشکر،
// سامانه مدیریت بورس";

//         var changeDetailsBuilder = new StringBuilder();
//         foreach (var detail in changeDetails)
//         {
//             var changeLine = $"- {detail.ChangeType}: {detail.OldValue} به {detail.NewValue}";
//             changeDetailsBuilder.AppendLine(changeLine);
//         }

//         var finalChangeDetails = changeDetailsBuilder.ToString();

//         return messageTemplate
//             .Replace("{userName}", userName)
//             .Replace("{changeDetails}", finalChangeDetails);
//     }

//     public string CreateBirthdayMessage(string userName)
//     {
//         var messageTemplate = @"
// موضوع: تولدت مبارک!

// کاربر گرامی {userName}،

// ما با خوشحالی تولد شما را تبریک می‌گوییم و آرزو می‌کنیم سالی پر از سلامتی، خوشبختی و موفقیت داشته باشید.

// به پاس این مناسبت، هدیه‌ای ویژه از طرف ما برای شما در نظر گرفته شده است. لطفاً به پنل کاربری خود مراجعه کنید تا از جزئیات آن مطلع شوید.

// با تشکر،
// سامانه مدیریت بورس";

//         return messageTemplate.Replace("{userName}", userName);
//     }

//     public string CreateOccasionMessage(string userName, string occasionName)
//     {
//         var messageTemplate = @"
// موضوع: تبریک {occasionName}

// کاربر گرامی {userName}،

// ما به مناسبت {occasionName} به شما تبریک می‌گوییم و آرزو می‌کنیم این مناسبت برای شما و خانواده‌تان پر از شادی و خوشحالی باشد.

// به پاس این مناسبت، هدیه‌ای ویژه از طرف ما برای شما در نظر گرفته شده است. لطفاً به پنل کاربری خود مراجعه کنید تا از جزئیات آن مطلع شوید.

// با تشکر،
// سامانه مدیریت بورس";

//         return messageTemplate
//             .Replace("{userName}", userName)
//             .Replace("{occasionName}", occasionName);
//     }

//     public string CreateCapitalIncreaseMessage(string userName, string companyName, string symbol, DateTime increaseDate, decimal increasePercentage, string sourceOfCapital)
//     {
//         var messageTemplate = @"
// موضوع: اطلاع‌رسانی پیرامون افزایش سرمایه نماد {symbol}

// کاربر گرامی {userName}،

// ما خوشحالیم به اطلاع شما برسانیم که افزایش سرمایه نماد {symbol} در تاریخ {increaseDate} انجام خواهد شد.

// جزئیات افزایش سرمایه:
// - نام شرکت: {companyName}
// - نماد سهام: {symbol}
// - تاریخ افزایش سرمایه: {increaseDate}
// - درصد افزایش سرمایه: {increasePercentage}
// - منبع افزایش سرمایه: {sourceOfCapital}

// لطفاً برای کسب اطلاعات بیشتر به پنل کاربری خود مراجعه کنید.

// با تشکر،
// سامانه مدیریت بورس";

//         return messageTemplate
//             .Replace("{userName}", userName)
//             .Replace("{companyName}", companyName)
//             .Replace("{symbol}", symbol)
//             .Replace("{increaseDate}", increaseDate.ToString("yyyy/MM/dd"))
//             .Replace("{increasePercentage}", $"{increasePercentage}%")
//             .Replace("{sourceOfCapital}", sourceOfCapital);
//     }

//     public string CreateCreditNotificationMessage(string userName, decimal creditAmount, DateTime creditDate, string paymentMethod)
//     {
//         var messageTemplate = @"
// موضوع: اطلاع‌رسانی میزان شارژ حساب

// کاربر گرامی {userName}،

// اعتبار حساب شما به مبلغ {creditAmount} تومان در تاریخ {creditDate} شارژ شده است.

// جزئیات شارژ:
// - مبلغ شارژ: {creditAmount} تومان
// - تاریخ شارژ: {creditDate}
// - روش پرداخت: {paymentMethod}

// در صورت نیاز به اطلاعات بیشتر، لطفاً به پنل کاربری خود مراجعه کنید.

// با تشکر،
// سامانه مدیریت بورس";

//         return messageTemplate
//             .Replace("{userName}", userName)
//             .Replace("{creditAmount}", creditAmount.ToString("N0"))
//             .Replace("{creditDate}", creditDate.ToString("yyyy/MM/dd"))
//             .Replace("{paymentMethod}", paymentMethod);
//     }

//     public string CreateGiftCreditMessage(string userName, decimal creditAmount, DateTime creditDate, string reasonForGift)
//     {
//         var messageTemplate = @"
// موضوع: دریافت اعتبار هدیه از کارگزار

// کاربر گرامی {userName}،

// ما با خوشحالی به اطلاع شما می‌رسانیم که مبلغ {creditAmount} تومان به عنوان هدیه از طرف کارگزار به حساب شما اضافه شده است.

// جزئیات هدیه:
// - مبلغ هدیه: {creditAmount} تومان
// - تاریخ دریافت هدیه: {creditDate}
// - علت دریافت هدیه: {reasonForGift}

// لطفاً برای مشاهده جزئیات بیشتر به پنل کاربری خود مراجعه کنید.

// با تشکر،
// سامانه مدیریت بورس";

//         return messageTemplate
//             .Replace("{userName}", userName)
//             .Replace("{creditAmount}", creditAmount.ToString("N0"))
//             .Replace("{creditDate}", creditDate.ToString("yyyy/MM/dd"))
//             .Replace("{reasonForGift}", reasonForGift);
//     }

//     public string CreateSystemAlertMessage(string userName, string issueType, DateTime startTime, string currentStatus, DateTime estimatedResolutionTime)
//     {
//         var messageTemplate = @"
// موضوع: اطلاع‌رسانی پیرامون اختلالات سیستم

// کاربر گرامی {userName}،

// این پیام به اطلاع شما می‌رساند که اختلالاتی در سیستم رخ داده است که ممکن است بر خدمات ما تأثیر بگذارد.

// جزئیات اختلال:
// - نوع اختلال: {issueType}
// - تاریخ و زمان شروع: {startTime}
// - وضعیت فعلی: {currentStatus}
// - تاریخ و زمان احتمالی رفع: {estimatedResolutionTime}

// تیم فنی ما در حال کار بر روی این موضوع است و ما در اسرع وقت شما را از رفع این مشکل مطلع خواهیم کرد. لطفاً از صبر و شکیبایی شما تشکر می‌کنیم.

// با تشکر،
// سامانه مدیریت بورس";

//         return messageTemplate
//             .Replace("{userName}", userName)
//             .Replace("{issueType}", issueType)
//             .Replace("{startTime}", startTime.ToString("yyyy/MM/dd HH:mm"))
//             .Replace("{currentStatus}", currentStatus)
//             .Replace("{estimatedResolutionTime}", estimatedResolutionTime.ToString("yyyy/MM/dd HH:mm"));
//     }

//     public string CreateOtpMessage(string userName, string otpCode, int validityPeriod)
//     {
//         var messageTemplate = @"
// موضوع: کد یک‌بار مصرف (OTP) شما

// کاربر گرامی {userName}،

// کد یک‌بار مصرف (OTP) شما برای ورود به سیستم / تأی

//     ";
//     }
//     public string CreateBrokerChangeNotificationMessage(string userName, string symbol, string newBrokerName, DateTime changeDate)
//     {
//         var messageTemplate = @"
// موضوع: اطلاع‌رسانی پیرامون تغییر کارگزار ناظر

// کاربر گرامی {userName}،

// ما به اطلاع شما می‌رسانیم که کارگزار ناظر سهام شما برای نماد {symbol} به {newBrokerName} تغییر یافته است.

// جزئیات تغییر:
// - نماد سهام: {symbol}
// - کارگزار ناظر جدید: {newBrokerName}
// - تاریخ تغییر: {changeDate}

// لطفاً برای کسب اطلاعات بیشتر به پنل کاربری خود مراجعه کنید. در صورت وجود هرگونه سؤال یا نیاز به راهنمایی، با پشتیبانی تماس بگیرید.

// با تشکر،
// سامانه مدیریت بورس";

//         var finalMessage = messageTemplate
//             .Replace("{userName}", userName)
//             .Replace("{symbol}", symbol)
//             .Replace("{newBrokerName}", newBrokerName)
//             .Replace("{changeDate}", changeDate.ToString("yyyy/MM/dd"));

//         return finalMessage;
//      }
//  }


    