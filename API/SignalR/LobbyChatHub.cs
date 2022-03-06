using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extentions;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class LobbyChatHub : Hub
    {
        private readonly LobbyChatTracker _lobbyChatTracker;
        public LobbyChatHub(LobbyChatTracker lobbyChatTracker)
        {
            _lobbyChatTracker = lobbyChatTracker;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var lobbyId = Int32.Parse(httpContext.Request.Query["lobbyId"]);

            try
            {
                await _lobbyChatTracker.MemberJoinedChat(lobbyId, Context.User.GetUserId());
            }
            catch (System.Exception err)
            {
                Console.WriteLine(err);
                await Clients.Caller.SendAsync("GetMessages", await _lobbyChatTracker.GetMessages(lobbyId));
                return;
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var lobbyId = await _lobbyChatTracker.GetLobbyIdFromUser(Context.User.GetUserId());
            if (lobbyId == 0) return;

            await base.OnDisconnectedAsync(exception);
        }
    }
}