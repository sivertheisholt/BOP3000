using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Lobbies;
using API.Entities.Lobbies;
using API.Extentions;
using API.Interfaces;
using API.Interfaces.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class LobbyChatHub : Hub
    {
        private readonly LobbyChatTracker _lobbyChatTracker;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public LobbyChatHub(LobbyChatTracker lobbyChatTracker, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _lobbyChatTracker = lobbyChatTracker;
        }

        public Task CreateChatTest(int lobbyId)
        {
            _lobbyChatTracker.CreateChat(lobbyId);

            return Task.CompletedTask;
        }

        public Task CreateChat(int lobbyId)
        {
            _lobbyChatTracker.CreateChat(lobbyId);

            return Task.CompletedTask;
        }


        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var lobbyId = Int32.Parse(httpContext.Request.Query["lobbyId"]);
            var uid = Context.User.GetUserId();

            if (!_lobbyChatTracker.MemberJoinedChat(lobbyId, uid)) await Clients.Group($"lobby_{lobbyId}").SendAsync("JoinedChat", uid);

            await AddToGroup($"lobby_{lobbyId}");
            await AddToGroup($"user_{uid}");

            await Clients.Caller.SendAsync("GetMessages", _lobbyChatTracker.GetMessages(lobbyId));

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(int lobbyId, string message)
        {
            var uid = Context.User.GetUserId();
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(uid);

            var Chatmessage = new Message
            {
                LobbyId = lobbyId,
                Uid = uid,
                DateSent = DateTime.Now,
                ChatMessage = message,
                Username = user.UserName
            };

            if (!_lobbyChatTracker.SendMessage(lobbyId, uid, Chatmessage)) return;

            var messageDto = _mapper.Map<MessageDto>(Chatmessage);

            await Clients.Group($"lobby_{lobbyId}").SendAsync("NewMessage", new List<MessageDto>() { messageDto });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var lobbyId = _lobbyChatTracker.GetLobbyIdFromUser(Context.User.GetUserId());
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