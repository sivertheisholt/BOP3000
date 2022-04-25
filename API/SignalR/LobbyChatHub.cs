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

        private class Checks
        {
            public Checks(bool checkUid = true, bool checkLobbyExists = true, bool checkIfMemberInLobby = false)
            {
                CheckUid = checkUid;
                CheckLobbyExists = checkLobbyExists;
                CheckIfMemberInLobby = checkIfMemberInLobby;
            }

            public bool CheckUid { get; set; } = true;
            public bool CheckLobbyExists { get; set; } = true;
            public bool CheckIfMemberInLobby { get; set; } = true;
        }

        private async Task<bool> GlobalChecks(Checks checks, int lobbyId = -1, int callerUid = -1, int targetUid = -1)
        {
            if (checks.CheckUid)
            {
                if (callerUid == -1)
                {
                    await Clients.Caller.SendAsync("ServerError", "Can't get UID");
                    return false;
                }
            }
            if (checks.CheckLobbyExists)
            {
                if (!_lobbyChatTracker.CheckIfChatExists(lobbyId))
                {
                    await Clients.Caller.SendAsync("ServerError", "Lobby doesn't exist");
                    return false;
                }
            }
            if (checks.CheckIfMemberInLobby)
            {
                if (!_lobbyChatTracker.CheckIfMemberInChat(lobbyId, targetUid))
                {
                    await Clients.Caller.SendAsync("ServerError", "Member is not in lobby");
                    return false;
                }
            }

            return true;
        }


        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var lobbyId = Int32.Parse(httpContext.Request.Query["lobbyId"]);
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(), lobbyId, uid, uid)) return;

            _lobbyChatTracker.MemberJoinedChat(lobbyId, uid);

            await Clients.Group($"lobby_{lobbyId}").SendAsync("JoinedChat", uid);

            await AddToGroup($"lobby_{lobbyId}");
            await AddToGroup($"user_{uid}");

            await Clients.Caller.SendAsync("GetMessages", _lobbyChatTracker.GetMessages(lobbyId));

            await base.OnConnectedAsync();
        }

        public async Task SendMessage(int lobbyId, string message)
        {
            var uid = Context.User.GetUserId();
            var user = await _unitOfWork.userRepository.GetUserByIdAsync(uid);

            if (!await GlobalChecks(new Checks(), lobbyId, uid, uid)) return;

            var Chatmessage = new Message
            {
                LobbyId = lobbyId,
                Uid = uid,
                DateSent = DateTime.Now,
                ChatMessage = message,
                Username = user.UserName
            };

            _lobbyChatTracker.SendMessage(lobbyId, uid, Chatmessage);

            var messageDto = _mapper.Map<MessageDto>(Chatmessage);

            await Clients.Group($"lobby_{lobbyId}").SendAsync("NewMessage", new List<MessageDto>() { messageDto });
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var uid = Context.User.GetUserId();
            var lobbyId = _lobbyChatTracker.GetLobbyIdFromUser(uid);

            if (!await GlobalChecks(new Checks(), lobbyId, uid, uid)) return;

            _lobbyChatTracker.MemberLeftChat(lobbyId, uid);

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