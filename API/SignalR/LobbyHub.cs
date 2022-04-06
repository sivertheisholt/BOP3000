using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extentions;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class LobbyHub : Hub
    {
        private readonly LobbyTracker _lobbyTracker;
        private readonly ILobbiesRepository _lobbiesRepository;
        private readonly IDiscordBotService _discordBotService;
        private static readonly Dictionary<int, CancellationTokenSource> LobbyStartingTask = new Dictionary<int, CancellationTokenSource>();
        private readonly IUserRepository _userRepository;
        public LobbyHub(LobbyTracker lobbyTracker, ILobbiesRepository lobbiesRepository, IDiscordBotService discordService, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _lobbiesRepository = lobbiesRepository;
            _discordBotService = discordService;
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
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("HostStarted", adminUid);

            CancellationTokenSource source = new CancellationTokenSource();
            var task = Task.Delay(30000, source.Token);
            LobbyStartingTask.Add(lobbyId, source);

            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                Console.Write("Lobby started before timer");
            }


            if (await _lobbyTracker.CheckReadyState(lobbyId))
            {
                var discordIds = new List<ulong>();
                foreach (var userId in await _lobbyTracker.GetMembersInLobby(lobbyId))
                {
                    discordIds.Add(await _userRepository.GetUserDiscordIdFromUid(userId));
                }
                var invitelink = await _discordBotService.CreateVoiceChannelForLobby(discordIds.ToArray());
                await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LobbyStarted", invitelink);
                await _lobbyTracker.FinishLobby(lobbyId);
            }
        }

        public async Task Accept(int lobbyId)
        {
            var uid = Context.User.GetUserId();
            await _lobbyTracker.AcceptReady(lobbyId, uid);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberAcceptedReady", uid);
            if (await _lobbyTracker.CheckIfAllReady(lobbyId))
            {
                Console.WriteLine("CANCELLING THE TIMER");
                LobbyStartingTask[lobbyId].Cancel();
            }
        }
        public async Task Decline(int lobbyId)
        {
            var uid = Context.User.GetUserId();
            await _lobbyTracker.DeclineReady(lobbyId, uid);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberDeclinedReady", uid);
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
            if (uid == -1) return;

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


