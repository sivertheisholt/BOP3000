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

        public async Task CreateLobbyTest(int lobbyId, int uid)
        {
            await _lobbyTracker.CreateLobby(lobbyId, uid);
        }

        public async Task CreateLobby(int lobbyId, int uid)
        {
            await _lobbyTracker.CreateLobby(lobbyId, uid);
            await AddToGroup($"lobby_{lobbyId.ToString()}");
        }

        public async Task AcceptMember(int lobbyId, int acceptedUid)
        {
            var uid = Context.User.GetUserId();
            var adminUid = await _lobbyTracker.GetLobbyAdmin(lobbyId);

            Console.WriteLine("Admin = " + adminUid + " uid = " + uid);

            if (adminUid != uid) return;

            var result = await _lobbyTracker.AcceptMember(lobbyId, acceptedUid);
            Console.WriteLine(result);

            if (!result) return;

            await AddToGroup($"lobby_{lobbyId.ToString()}");
            await Clients.Group($"user_{acceptedUid.ToString()}").SendAsync("Accepted");
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberAccepted", new List<int>() { lobbyId, acceptedUid });
        }
        public async Task DeclineMember(int lobbyId, int declinedUid)
        {
            var uid = Context.User.GetUserId();
            var adminUid = await _lobbyTracker.GetLobbyAdmin(lobbyId);

            if (adminUid != uid) return;

            if (!await _lobbyTracker.DeclineMember(lobbyId, declinedUid))
            {
                Console.WriteLine("Could not decline member");
                return;
            }

            await Clients.Group($"user_{declinedUid.ToString()}").SendAsync("Declined");
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberDeclined", new List<int>() { lobbyId, declinedUid });
        }
        public async Task BanMember(int lobbyId, int bannedUid)
        {
            var uid = Context.User.GetUserId();
            var adminUid = await _lobbyTracker.GetLobbyAdmin(lobbyId);

            if (adminUid != uid) return;

            if (!await _lobbyTracker.BanMember(lobbyId, bannedUid)) return;
            await Clients.Group($"user_{bannedUid.ToString()}").SendAsync("Banned");
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberBanned", new List<int>() { lobbyId, bannedUid });
        }

        public async Task KickMember(int lobbyId, int kickedUid)
        {
            var uid = Context.User.GetUserId();
            var adminUid = await _lobbyTracker.GetLobbyAdmin(lobbyId);

            if (adminUid != uid) return;

            if (!await _lobbyTracker.KickMember(lobbyId, kickedUid)) return;
            await Clients.Group($"user_{kickedUid.ToString()}").SendAsync("Kicked");
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberKicked", new List<int>() { lobbyId, kickedUid });
        }
        public async Task JoinQueue(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await _lobbyTracker.JoinQueue(lobbyId, uid)) return;

            if (!await _lobbyTracker.CheckIfMemberIsBanned(lobbyId, uid))

                await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("JoinedLobbyQueue", uid);
        }
        public async Task LeaveQueue(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await _lobbyTracker.CheckIfMemberInQueue(lobbyId, uid)) return;

            await _lobbyTracker.MemberLeftQueueLobby(lobbyId, uid);

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LeftQueue", uid);
        }
        public async Task LeaveLobby(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await _lobbyTracker.CheckIfMemberInLobby(lobbyId, uid)) return;

            await _lobbyTracker.MemberLeftLobby(lobbyId, uid);

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LeftLobby", uid);
        }

        public async Task StartCheck(int lobbyId)
        {
            var uid = Context.User.GetUserId();
            var adminUid = await _lobbyTracker.GetLobbyAdmin(lobbyId);

            if (adminUid != uid) return;

            await _lobbyTracker.StartCheck(lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("HostStarted");

            await Task.Delay(5000);

            if (await _lobbyTracker.CheckReadyState(lobbyId))
            {
                await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LobbyStarted");
                await _lobbyTracker.FinishLobby(lobbyId);
            }
        }

        public async Task Accept(int lobbyId)
        {
            var uid = Context.User.GetUserId();
            await _lobbyTracker.AcceptReady(lobbyId, uid);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberAccepted");
        }
        public async Task Decline(int lobbyId)
        {
            var uid = Context.User.GetUserId();
            await _lobbyTracker.DeclineReady(lobbyId, uid);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberDeclined");
        }

        public async Task GetQueueMembers(int lobbyId)
        {
            var members = await _lobbyTracker.GetMembersInQueueLobby(lobbyId);
            await Clients.Caller.SendAsync("QueueMembers", members);
        }
        public async Task GetLobbyMembers(int lobbyId)
        {
            var members = await _lobbyTracker.GetMembersInLobby(lobbyId);
            await Clients.Caller.SendAsync("LobbyMembers", members);
        }

        public override async Task OnConnectedAsync()
        {
            var uid = Context.User.GetUserId();

            //Testing lobby 1 admin user 1
            if (Context.User.GetUserId() == 1)
            {
                await AddToGroup($"lobby_{1}");
            }

            await AddToGroup($"user_{uid}");
            if (await _lobbyTracker.CheckIfMemberInAnyLobby(uid))
            {
                var lobbyId = await _lobbyTracker.GetLobbyIdFromUser(uid);
                if (await _lobbyTracker.CheckIfMemberInLobby(lobbyId, uid))
                {
                    await AddToGroup($"lobby_{lobbyId}");
                }
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


