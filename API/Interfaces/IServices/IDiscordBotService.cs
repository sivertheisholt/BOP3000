using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces.IServices
{
    public interface IDiscordBotService
    {
        Task<string> CreateVoiceChannelForLobby(ulong[] userIds, string channelName);

        Task<bool> CheckIfUserInServer(ulong userId);
    }
}