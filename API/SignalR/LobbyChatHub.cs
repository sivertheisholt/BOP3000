using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Lobbies;
using API.Entities.Lobbies;
using API.Extentions;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class LobbyChatHub : Hub
    {
        private readonly LobbyChatTracker _lobbyChatTracker;
        private readonly IMapper _mapper;
        public LobbyChatHub(LobbyChatTracker lobbyChatTracker, IMapper mapper)
        {
            _mapper = mapper;
            _lobbyChatTracker = lobbyChatTracker;
        }

        public async Task CreateChat(int lobbyId)
        {
            await _lobbyChatTracker.CreateChat(lobbyId);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var lobbyId = Int32.Parse(httpContext.Request.Query["lobbyId"]);
            Console.WriteLine(lobbyId);
            var uid = Context.User.GetUserId();

            if (!await _lobbyChatTracker.CheckIfChatExists(1))
            {
                Console.WriteLine("CHAT DOES NOT EXIST");
                await _lobbyChatTracker.CreateChat(1);
            }

            await AddToGroup($"user_{uid}");
            if (!await _lobbyChatTracker.CheckIfUserInChat(uid))
            {
                if (await _lobbyChatTracker.MemberJoinedChat(lobbyId, uid))
                {
                    await AddToGroup($"lobby_{lobbyId}");
                    await Clients.OthersInGroup($"lobby_{lobbyId}").SendAsync("JoinedChat", uid);
                }
                await Clients.Caller.SendAsync("GetMessages", await _lobbyChatTracker.GetMessages(lobbyId));
            }
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(int lobbyId, string message)
        {
            var httpContext = Context.GetHttpContext();
            var uid = Context.User.GetUserId();

            var Chatmessage = new Message
            {
                LobbyId = lobbyId,
                Uid = uid,
                DateSent = DateTime.Now,
                ChatMessage = message
            };

            if (await _lobbyChatTracker.SendMessage(lobbyId, uid, Chatmessage))
            {
                await Clients.Group($"lobby_{lobbyId}").SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }


        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var lobbyId = await _lobbyChatTracker.GetLobbyIdFromUser(Context.User.GetUserId());
            if (lobbyId == 0) return;

            await base.OnDisconnectedAsync(exception);
        }

        private async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        private async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}