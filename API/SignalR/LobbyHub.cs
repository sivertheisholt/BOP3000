using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Activities;
using API.Entities.Lobbies;
using API.Extentions;
using API.Interfaces;
using API.Interfaces.IRepositories;
using API.Interfaces.IServices;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class LobbyHub : Hub
    {
        private readonly LobbyTracker _lobbyTracker;
        private readonly IDiscordBotService _discordBotService;
        private static readonly Dictionary<int, CancellationTokenSource> LobbyStartingTask = new Dictionary<int, CancellationTokenSource>();
        private readonly LobbyChatTracker _lobbyChatTracker;
        private readonly IUnitOfWork _unitOfWork;
        public LobbyHub(LobbyTracker lobbyTracker, IDiscordBotService discordService, LobbyChatTracker lobbyChatTracker, IUnitOfWork unitOfWork)
        {
            _discordBotService = discordService;
            _lobbyTracker = lobbyTracker;
            _lobbyChatTracker = lobbyChatTracker;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateLobbyTest(Lobby lobby, int uid)
        {
            await _lobbyTracker.CreateLobby(lobby, uid);
        }

        public async Task CreateLobby(Lobby lobby, int uid)
        {
            await _lobbyTracker.CreateLobby(lobby, uid);
            await AddToGroup($"lobby_{lobby.Id.ToString()}");
        }

        public async Task AcceptMember(int lobbyId, int acceptedUid)
        {
            var uid = Context.User.GetUserId();

            if (await _lobbyTracker.CheckIfLobbyFull(lobbyId))
            {
                await Clients.Caller.SendAsync("FullLobby");
                return;
            }

            var result = await _lobbyTracker.AcceptMember(lobbyId, acceptedUid, uid);

            if (!result) return;

            var activityLog = new ActivityLog
            {
                Date = DateTime.Now,
                LobbyId = lobbyId,
                AppUserId = acceptedUid,
                ActivityId = 3,
            };

            _unitOfWork.activitiesRepository.AddActivityLog(activityLog);

            await _unitOfWork.Complete();

            await AddToGroup($"lobby_{lobbyId.ToString()}");
            await Clients.Group($"user_{acceptedUid.ToString()}").SendAsync("Accepted", lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberAccepted", new List<int>() { lobbyId, acceptedUid });
        }
        public async Task DeclineMember(int lobbyId, int declinedUid)
        {
            var uid = Context.User.GetUserId();

            var result = await _lobbyTracker.DeclineMember(lobbyId, declinedUid, uid);

            if (!result) return;

            await Clients.Group($"user_{declinedUid.ToString()}").SendAsync("Declined", lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberDeclined", new List<int>() { lobbyId, declinedUid });
        }

        public async Task BanMember(int lobbyId, int bannedUid)
        {
            var uid = Context.User.GetUserId();

            var result = await _lobbyTracker.BanMember(lobbyId, bannedUid, uid);

            if (!result) return;

            await Clients.Group($"user_{bannedUid.ToString()}").SendAsync("Banned", lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberBanned", new List<int>() { lobbyId, bannedUid });
        }

        public async Task KickMember(int lobbyId, int kickedUid)
        {
            var uid = Context.User.GetUserId();

            var result = await _lobbyTracker.KickMember(lobbyId, kickedUid, uid);

            if (!result) return;

            await Clients.Group($"user_{kickedUid.ToString()}").SendAsync("Kicked", lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberKicked", new List<int>() { lobbyId, kickedUid });
        }

        public async Task JoinQueue(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (await _lobbyTracker.CheckIfMemberIsBanned(lobbyId, uid)) return;

            if (!await _unitOfWork.userRepository.CheckIfDiscordConnected(uid)) return;

            var discordId = await _unitOfWork.userRepository.GetUserDiscordIdFromUid(uid);

            if (!await _discordBotService.CheckIfUserInServer(discordId))
            {
                await Clients.Caller.SendAsync("NotInDiscordServer");
                return;
            }

            if (!await _lobbyTracker.JoinQueue(lobbyId, uid)) return;

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("JoinedLobbyQueue", uid);
            await Clients.Caller.SendAsync("InQueue", lobbyId);
        }

        public async Task LeaveQueue(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await _lobbyTracker.CheckIfMemberInQueue(lobbyId, uid)) return;

            await _lobbyTracker.MemberLeftQueueLobby(lobbyId, uid);

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LeftQueue", uid);
            await Clients.Caller.SendAsync("CancelQueue");

        }

        public async Task LeaveLobby(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await _lobbyTracker.CheckIfMemberInLobby(lobbyId, uid)) return;

            await _lobbyTracker.MemberLeftLobby(lobbyId, uid);

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LeftLobby", uid);
            await Clients.Caller.SendAsync("CancelLobby");
        }
        
        public async Task StartCheck(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            var result = await _lobbyTracker.StartCheck(lobbyId, uid);

            if (!result) return;

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("HostStarted", uid);

            CancellationTokenSource source = new CancellationTokenSource();
            var task = Task.Delay(30000, source.Token);
            LobbyStartingTask.Add(lobbyId, source);

            try
            {
                task.Wait();
            }
            catch (AggregateException)
            {
                Console.Write("Lobby started before timer");
            }

            if (await _lobbyTracker.CheckReadyState(lobbyId))
            {
                await AllReady(lobbyId);
            }
            else
            {
                await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LobbyCancelled");
            }
        }

        public async Task AcceptCheck(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await _lobbyTracker.AcceptReady(lobbyId, uid)) return;

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberAcceptedReady", uid);

            if (await _lobbyTracker.CheckIfAllReady(lobbyId)) LobbyStartingTask[lobbyId].Cancel();
        }

        public async Task DeclineCheck(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await _lobbyTracker.DeclineReady(lobbyId, uid)) return;
            if (!await _lobbyTracker.CancelCheck(lobbyId)) return;

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
        public async Task EndLobby(int lobbyId)
        {
            var queueUsers = await _lobbyTracker.GetMembersInQueueLobby(lobbyId);
            await _lobbyTracker.FinishLobby(lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("EndedLobby");
            foreach (var user in queueUsers)
            {
                await Clients.Group($"user_{user}").SendAsync("EndedLobby", lobbyId);
            }
            var lobby = await _unitOfWork.lobbiesRepository.GetLobbyAsync(lobbyId);
            _unitOfWork.lobbiesRepository.Delete(lobby);
            await _unitOfWork.Complete();
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


        private async Task AllReady(int lobbyId)
        {
            await StartLobby(lobbyId);
            await FinishLobby(lobbyId);
        }

        private async Task StartLobby(int lobbyId)
        {
            var discordIds = new List<ulong>();
            foreach (var userId in await _lobbyTracker.GetMembersInLobby(lobbyId))
            {
                discordIds.Add(await _unitOfWork.userRepository.GetUserDiscordIdFromUid(userId));
            }

            var adminName = await _unitOfWork.userRepository.GetUsernameFromId(await _lobbyTracker.GetLobbyAdmin(lobbyId));
            var channelName = $"{adminName}'s lobby";
            var invitelink = await _discordBotService.CreateVoiceChannelForLobby(discordIds.ToArray(), channelName);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LobbyStarted", invitelink);
            var queueUsers = await _lobbyTracker.GetMembersInQueueLobby(lobbyId);
            foreach (var user in queueUsers)
            {
                await Clients.Group($"user_{user}").SendAsync("QueueKicked");
            }
        }
        private async Task FinishLobby(int lobbyId)
        {
            var lobby = await _unitOfWork.lobbiesRepository.GetLobbyAsync(lobbyId);
            var users = await _lobbyTracker.GetMembersInLobby(lobbyId);
            lobby.Finished = true;
            lobby.Users = users;
            lobby.FinishedDate = DateTime.Now;
            lobby.Log = new Log
            {
                Messages = await _lobbyChatTracker.GetMessages(lobbyId)
            };

            await _lobbyTracker.FinishLobby(lobbyId);

            var finishTask = Task.Delay(20 * 1000);
            await finishTask;
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("RedirectFinished", lobbyId);

            _unitOfWork.lobbiesRepository.Update(lobby);

            await _lobbyChatTracker.LobbyChatDone(lobbyId);

            foreach (var uid in users)
            {
                var activityLog = new ActivityLog
                {
                    Date = DateTime.Now,
                    LobbyId = lobbyId,
                    AppUserId = uid,
                    ActivityId = 1,
                };

                _unitOfWork.activitiesRepository.AddActivityLog(activityLog);
            }

            await _unitOfWork.Complete();
        }
    }
}


