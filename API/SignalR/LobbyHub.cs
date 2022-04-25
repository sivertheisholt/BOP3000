using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Lobbies;
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

        public Task CreateLobbyTest(Lobby lobby, int uid)
        {
            _lobbyTracker.CreateLobby(lobby, uid);
            return Task.CompletedTask;
        }

        public Task CreateLobby(Lobby lobby, int uid)
        {
            _lobbyTracker.CreateLobby(lobby, uid);
            return Task.CompletedTask;
        }

        private class Checks
        {
            public Checks(bool checkUid = true, bool checkLobbyExists = true, bool checkMemberInLobby = false, bool checkMemberInQueue = false, bool checkMemberAdmin = false, bool checkIfLobbyFull = false, bool checkIfMemberIsBanned = false)
            {
                CheckUid = checkUid;
                CheckLobbyExists = checkLobbyExists;
                CheckMemberInLobby = checkMemberInLobby;
                CheckMemberInQueue = checkMemberInQueue;
                CheckMemberAdmin = checkMemberAdmin;
                CheckIfLobbyFull = checkIfLobbyFull;
                CheckIfMemberIsBanned = checkIfMemberIsBanned;
            }

            public bool CheckUid { get; set; } = true;
            public bool CheckLobbyExists { get; set; } = true;
            public bool CheckMemberInLobby { get; set; } = false;
            public bool CheckMemberInQueue { get; set; } = false;
            public bool CheckMemberAdmin { get; set; } = false;
            public bool CheckIfLobbyFull { get; set; } = false;
            public bool CheckIfMemberIsBanned { get; set; } = false;

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
                if (!_lobbyTracker.CheckIfLobbyExists(lobbyId))
                {
                    await Clients.Caller.SendAsync("ServerError", "Lobby doesn't exist");
                    return false;
                }
            }
            if (checks.CheckMemberInLobby)
            {
                if (!_lobbyTracker.CheckIfMemberInLobby(lobbyId, targetUid))
                {
                    await Clients.Caller.SendAsync("ServerError", "Member is not in lobby");
                    return false;
                }
            }
            if (checks.CheckMemberInQueue)
            {
                if (!_lobbyTracker.CheckIfMemberInQueue(lobbyId, targetUid))
                {
                    await Clients.Caller.SendAsync("ServerError", "Member is not in queue");
                    return false;
                }
            }
            if (checks.CheckMemberAdmin)
            {
                if (!_lobbyTracker.CheckIfMemberIsAdmin(lobbyId, callerUid))
                {
                    await Clients.Caller.SendAsync("ServerError", "Member is not admin");
                    return false;
                }
            }
            if (checks.CheckIfLobbyFull)
            {
                if (_lobbyTracker.CheckIfLobbyFull(lobbyId))
                {
                    await Clients.Caller.SendAsync("FullLobby", "Lobby is full");
                    return false;
                }
            }
            if (checks.CheckIfMemberIsBanned)
            {
                if (_lobbyTracker.CheckIfMemberIsBanned(lobbyId, targetUid))
                {
                    await Clients.Caller.SendAsync("BannedQueue");
                    return false;
                }
            }

            return true;
        }

        public async Task AcceptMember(int lobbyId, int acceptedUid)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(checkIfLobbyFull: true, checkMemberInQueue: true), lobbyId: lobbyId, callerUid: uid, targetUid: acceptedUid)) return;

            _lobbyTracker.AcceptMember(lobbyId, acceptedUid);

            var activityLog = new ActivityLog
            {
                Date = DateTime.Now,
                LobbyId = lobbyId,
                AppUserId = acceptedUid,
                ActivityId = 3,
            };

            _unitOfWork.activitiesRepository.AddActivityLog(activityLog);

            await Clients.Group($"user_{acceptedUid.ToString()}").SendAsync("Accepted", lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberAccepted", new List<int>() { lobbyId, acceptedUid });

            await _unitOfWork.Complete();
        }
        public async Task DeclineMember(int lobbyId, int declinedUid)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(checkMemberInQueue: true, checkMemberAdmin: true), lobbyId, uid, declinedUid)) return;

            _lobbyTracker.DeclineMember(lobbyId, declinedUid);

            await Clients.Group($"user_{declinedUid.ToString()}").SendAsync("Declined", lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberDeclined", new List<int>() { lobbyId, declinedUid });
        }

        public async Task BanMember(int lobbyId, int bannedUid)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(checkMemberAdmin: true), lobbyId, uid, bannedUid)) return;

            _lobbyTracker.BanMember(lobbyId, bannedUid);

            await Clients.Group($"user_{bannedUid.ToString()}").SendAsync("Banned", lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberBanned", new List<int>() { lobbyId, bannedUid });
        }

        public async Task KickMember(int lobbyId, int kickedUid)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(checkMemberInLobby: true, checkMemberAdmin: true), lobbyId, uid, kickedUid)) return;

            _lobbyTracker.KickMember(lobbyId, kickedUid);

            await Clients.Group($"user_{kickedUid.ToString()}").SendAsync("Kicked", lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberKicked", new List<int>() { lobbyId, kickedUid });
        }

        public async Task JoinQueue(int lobbyId)
        {
            var uid = Context.User.GetUserId();
            if (!await GlobalChecks(new Checks(checkIfMemberIsBanned: true), lobbyId, uid, uid)) return;

            if (!await _unitOfWork.userRepository.CheckIfDiscordConnected(uid))
            {
                await Clients.Caller.SendAsync("DiscordNotConnected");
                return;
            };

            var discordId = await _unitOfWork.userRepository.GetUserDiscordIdFromUid(uid);

            if (!await _discordBotService.CheckIfUserInServer(discordId))
            {
                await Clients.Caller.SendAsync("NotInDiscordServer");
                return;
            }

            _lobbyTracker.JoinQueue(lobbyId, uid);

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("JoinedLobbyQueue", uid);
            await Clients.Caller.SendAsync("InQueue", lobbyId);
        }

        public async Task LeaveQueue(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(checkMemberInQueue: true), lobbyId, uid, uid)) return;

            _lobbyTracker.MemberLeftQueueLobby(lobbyId, uid);

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LeftQueue", uid);
            await Clients.Caller.SendAsync("CancelQueue");

        }

        public async Task LeaveLobby(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(checkMemberInLobby: true), lobbyId, uid, uid)) return;

            _lobbyTracker.MemberLeftLobby(lobbyId, uid);

            await RemoveFromGroup($"lobby_{lobbyId.ToString()}");
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LeftLobby", uid);
            await Clients.Caller.SendAsync("CancelLobby");
        }

        public async Task StartCheck(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(checkMemberAdmin: true), lobbyId, uid, uid)) return;

            _lobbyTracker.StartCheck(lobbyId, uid);

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("HostStarted", new List<int>() { uid, lobbyId });

            CancellationTokenSource source = new CancellationTokenSource();
            lock (LobbyStartingTask) LobbyStartingTask.Add(lobbyId, source);
            var task = Task.Delay(30000, source.Token);

            try
            {
                task.Wait();
            }
            catch (AggregateException)
            {
                Console.Write("Lobby started before timer");
            }

            lock (LobbyStartingTask) LobbyStartingTask.Remove(lobbyId);

            if (!_lobbyTracker.CheckReadyState(lobbyId))
            {
                await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LobbyCancelled");
                return;
            }

            if (!_lobbyTracker.CheckIfAllReady(lobbyId))
            {
                await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LobbyCancelled");
                return;
            }

            await AllReady(lobbyId);
        }

        public async Task AcceptCheck(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(), lobbyId, uid, uid)) return;

            _lobbyTracker.AcceptReady(lobbyId, uid);

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberAcceptedReady", uid);

            if (_lobbyTracker.CheckIfAllReady(lobbyId)) LobbyStartingTask[lobbyId].Cancel();
        }

        public async Task DeclineCheck(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(), lobbyId, uid, uid)) return;

            _lobbyTracker.DeclineReady(lobbyId, uid);
            _lobbyTracker.CancelCheck(lobbyId);

            LobbyStartingTask[lobbyId].Cancel();

            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("MemberDeclinedReady", uid);
        }

        public async Task GetQueueMembers(int lobbyId)
        {
            if (!await GlobalChecks(new Checks(checkUid: false), lobbyId: lobbyId)) return;

            var members = _lobbyTracker.GetMembersInQueueLobby(lobbyId);
            await Clients.Caller.SendAsync("QueueMembers", members);
        }

        public async Task GetLobbyMembers(int lobbyId)
        {
            if (!await GlobalChecks(new Checks(checkUid: false), lobbyId: lobbyId)) return;

            var members = _lobbyTracker.GetMembersInLobby(lobbyId);
            await Clients.Caller.SendAsync("LobbyMembers", members);
        }

        public async Task AcceptedResponse(int lobbyId)
        {
            await AddToGroup($"lobby_{lobbyId.ToString()}");
        }
        public async Task KickedResponse(int lobbyId)
        {
            await RemoveFromGroup($"lobby_{lobbyId.ToString()}");
        }
        public async Task CreatedLobby(int lobbyId)
        {
            await AddToGroup($"lobby_{lobbyId.ToString()}");
        }
        public async Task EndLobby(int lobbyId)
        {
            var uid = Context.User.GetUserId();

            if (!await GlobalChecks(new Checks(checkUid: false), lobbyId: lobbyId)) return;

            var queueUsers = _lobbyTracker.GetMembersInQueueLobby(lobbyId);
            _lobbyTracker.FinishLobby(lobbyId);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("EndedLobby", lobbyId);
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
            if (_lobbyTracker.CheckIfMemberInAnyLobby(uid))
            {
                var lobbyId = _lobbyTracker.GetLobbyIdFromUser(uid);
                if (_lobbyTracker.CheckIfMemberInLobby(lobbyId, uid))
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
            foreach (var userId in _lobbyTracker.GetMembersInLobby(lobbyId))
            {
                discordIds.Add(await _unitOfWork.userRepository.GetUserDiscordIdFromUid(userId));
            }

            var adminName = await _unitOfWork.userRepository.GetUsernameFromId(_lobbyTracker.GetLobbyAdmin(lobbyId));
            var channelName = $"{adminName}'s lobby";
            var invitelink = await _discordBotService.CreateVoiceChannelForLobby(discordIds.ToArray(), channelName);
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("LobbyStarted", invitelink);
            var queueUsers = _lobbyTracker.GetMembersInQueueLobby(lobbyId);
            foreach (var user in queueUsers)
            {
                await Clients.Group($"user_{user}").SendAsync("QueueKicked");
            }
        }
        private async Task FinishLobby(int lobbyId)
        {
            var lobby = await _unitOfWork.lobbiesRepository.GetLobbyAsync(lobbyId);
            var users = _lobbyTracker.GetMembersInLobby(lobbyId);
            lobby.Finished = true;
            lobby.Users = users;
            lobby.FinishedDate = DateTime.Now;
            lobby.Log = new Log
            {
                Messages = _lobbyChatTracker.GetMessages(lobbyId)
            };

            _lobbyTracker.FinishLobby(lobbyId);

            var finishTask = Task.Delay(20 * 1000);
            await finishTask;
            await Clients.Group($"lobby_{lobbyId.ToString()}").SendAsync("RedirectFinished", lobbyId);

            _unitOfWork.lobbiesRepository.Update(lobby);

            _lobbyChatTracker.LobbyChatDone(lobbyId);

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

            foreach (var userId in users)
            {
                var user = await _unitOfWork.userRepository.GetUserByIdAsync(userId);
                user.AppUserProfile.AppUserData.FinishedLobbies.Add(lobbyId);
                _unitOfWork.userRepository.Update(user);
            }

            await _unitOfWork.Complete();
        }
    }
}


