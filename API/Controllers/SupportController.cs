using API.DTOs.Support;
using API.Interfaces.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SupportController : BaseApiController
    {
        private readonly IEmailService _email;
        public SupportController(IMapper mapper, IEmailService email) : base(mapper)
        {
            _email = email;
        }

        [HttpPost("create-ticket")]
        public async Task<ActionResult> CreateTicket(NewTicketDto ticketDto)
        {
            await _email.SendNewTicketMail(ticketDto);
            return Ok();
        }
    }
}