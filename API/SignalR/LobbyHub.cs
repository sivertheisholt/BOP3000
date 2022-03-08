using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extentions;
using API.Interfaces.IRepositories;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class LobbyHub : Hub
    {
        private readonly LobbyTracker _lobbyTracker;
        private readonly ILobbiesRepository _lobbiesRepository;
        public LobbyHub(LobbyTracker lobbyTracker, ILobbiesRepository lobbiesRepository)
        {
            _lobbiesRepository = lobbiesRepository;
            _lobbyTracker = lobbyTracker;
        }

        public async Task CreateLobby(int lobbyId)
        {
            var uid = Context.User.GetUserId();
            await _lobbyTracker.CreateLobby(lobbyId, uid);
            await AddToGroup($"lobby_{lobbyId.ToString()}");
        }

        public async Task AcceptMember(int lobbyId, int acceptedUid)
        {
            var uid = Context.User.GetUserId();
            var adminUid = await _lobbyTracker.GetLobbyAdmin(lobbyId);

            if (adminUid != uid) return;

            if (!await _lobbyTracker.AcceptMember(lobbyId, uid)) return;

            await AddToGroup($"lobby_{lobbyId.ToString()}");
            await Clients.Group($"user_{acceptedUid.ToString()}").SendAsync("Accepted");
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberAccepted", uid);
        }
        public async Task DeclineMember(int lobbyId, int declinedUid)
        {
            var uid = Context.User.GetUserId();
            var adminUid = await _lobbyTracker.GetLobbyAdmin(lobbyId);

            if (adminUid != uid) return;

            if (!await _lobbyTracker.DeclineMember(lobbyId, declinedUid)) return;

            await Clients.Group($"user_{declinedUid.ToString()}").SendAsync("Declined");
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberDeclined", uid);
        }
        public async Task BanMember(int lobbyId, int declinedUid)
        {
            var uid = Context.User.GetUserId();
            var adminUid = await _lobbyTracker.GetLobbyAdmin(lobbyId);

            if (adminUid != uid) return;

            if (!await _lobbyTracker.BanMember(lobbyId, declinedUid)) return;
            await Clients.Group($"user_{declinedUid.ToString()}").SendAsync("Banned");
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberBanned", uid);
        }
        public async Task JoinQueue(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await _lobbyTracker.JoinQueue(lobbyId, uid)) return;

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("JoinedLobbyQueue", uid);
        }

        public async Task GetQueueMembers(int lobbyId)
        {
            var members = await _lobbyTracker.GetMembersInQueueLobby(lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("QueueMembers", members);
        }
        public async Task GetLobbyMembers(int lobbyId)
        {
            var members = await _lobbyTracker.GetMembersInLobby(lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LobbyMembers", members);
        }

        public override async Task OnConnectedAsync()
        {
            await AddToGroup($"user_{Context.User.GetUserId().ToString()}");

            if (Context.User.GetUserId() == 1)
            {
                if (!await _lobbyTracker.CheckIfLobbyExists(1))
                {
                    Console.WriteLine("LOBBY DOES NOT EXIST");
                    await _lobbyTracker.CreateLobby(1, 1);
                }
                await AddToGroup($"lobby_1");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
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


