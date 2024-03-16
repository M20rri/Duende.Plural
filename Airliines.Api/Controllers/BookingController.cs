using Airlines.Api.Models;
using Airlines.Api.Services;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace Airlines.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseController
    {
        private readonly IMessageProducer _broker;
        public BookingController(IMessageProducer broker) => _broker = broker;

        [HttpPost("serve-ticket")]
        public IActionResult ServeTicketAsync([FromBody] Ticket model)
        {
            _broker.SendMessage(model);
            return CustomResult("Booking Successfully");
        }
    }
}
