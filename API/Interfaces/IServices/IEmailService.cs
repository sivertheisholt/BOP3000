using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Support;

namespace API.Interfaces.IServices
{
    public interface IEmailService
    {
        bool SendForgottenPasswordMail(string token, string email);

        Task SendNewTicketMail(NewTicketDto ticketDto);
    }
}