using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Applications;

namespace API.Interfaces.IClients
{
    public interface IDiscordApiClient
    {
        Task<DiscordUserDto> GetUserObjectFromToken(string token);
    }
}