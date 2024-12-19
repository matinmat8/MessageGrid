using BMO.API.Core.Entities.Requests;
using BMO.API.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BMO.API.MessageGrid.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmsController : ControllerBase
    {
        private readonly INotifier<DailyTradeSummaryNotifierRequest> _dailyNotifier;

        public SmsController(INotifier<DailyTradeSummaryNotifierRequest> notifier)
        {
            _dailyNotifier = notifier;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendSms([FromBody] DailyTradeSummaryNotifierRequest request)
        {

            // var parameters = new List<KeyValuePair<string, object>>{};

            // foreach (var variable in request.variables)
            // {
            //     parameters.Add(new KeyValuePair<string, object>(variable.Key, variable.Value));
            // }

            try
            {

                await _dailyNotifier.SetMessage(request);
                // await _smsSender.Send(request.variables);
                return Ok("SMS sent successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
