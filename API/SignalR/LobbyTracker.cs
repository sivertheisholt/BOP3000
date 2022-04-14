using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Lobbies;
using API.Entities.Lobbies.LobbyTracking;
using API.Interfaces.IRepositories;

namespace API.SignalR
{
    public class LobbyTracker
    {
        private static readonly Dictionary<int, LobbyStatusTracker> Lobby = new Dictionary<int, LobbyStatusTracker>();
        private static readonly Dictionary<int, int> MemberTracker = new Dictionary<int, int>();

        public Task CreateLobby(Lobby lobby, int adminUid)
        {
            var lobbyTracker = new LobbyStatusTracker
            {
                AdminUid = adminUid,
                BannedUsers = new List<int>(),
                LobbyId = lobby.Id,
                UsersLobby = new List<LobbyUser>()
                    {
                        new LobbyUser
                        {
                            Uid = adminUid,
                            Ready = false
                        }
                    },
                UsersQueue = new List<int>(),
                MaxUsers = lobby.MaxUsers
            };

            lock (Lobby) Lobby.Add(lobby.Id, lobbyTracker);
            lock (MemberTracker) MemberTracker.Add(adminUid, lobby.Id);

            return Task.CompletedTask;
        }

        public Task<bool> JoinQueue(int lobbyId, int uid)
        {
            if (MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            lock (Lobby) Lobby[lobbyId].UsersQueue.Add(uid);
            lock (MemberTracker) MemberTracker.Add(uid, lobbyId);

            return Task.FromResult(true);
        }

        public Task<bool> AcceptMember(int lobbyId, int uid, int adminUid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            if (!Lobby[lobbyId].UsersQueue.Contains(uid)) return Task.FromResult(false);

            if (Lobby[lobbyId].AdminUid != adminUid) return Task.FromResult(false);

            var lobbyUser = new LobbyUser
            {
                Ready = false,
                Uid = uid
            };

            lock (Lobby)
            {
                Lobby[lobbyId].UsersLobby.Add(lobbyUser);
                Lobby[lobbyId].UsersQueue.Remove(uid);
            }

            return Task.FromResult(true);
        }

        public Task<bool> DeclineMember(int lobbyId, int uid, int adminUid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            if (Lobby[lobbyId].AdminUid != adminUid) return Task.FromResult(false);

            lock (Lobby) Lobby[lobbyId].UsersQueue.Remove(uid);

            lock (MemberTracker) MemberTracker.Remove(uid);

            return Task.FromResult(true);
        }

        public Task<bool> KickMember(int lobbyId, int uid, int adminUid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            if (Lobby[lobbyId].AdminUid != adminUid) return Task.FromResult(false);

            var userLobby = Lobby[lobbyId].UsersLobby.Where(user => user.Uid == uid).FirstOrDefault();

            lock (Lobby) Lobby[lobbyId].UsersLobby.Remove(userLobby);

            lock (MemberTracker) MemberTracker.Remove(uid);

            return Task.FromResult(true);
        }

        public Task<bool> MemberLeftLobby(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            var userLobby = Lobby[lobbyId].UsersLobby.Where(user => user.Uid == uid).FirstOrDefault();

            lock (Lobby) Lobby[lobbyId].UsersLobby.Remove(userLobby);
            lock (MemberTracker) MemberTracker.Remove(uid);

            return Task.FromResult(true);
        }

        public Task<bool> MemberLeftQueueLobby(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            lock (Lobby) Lobby[lobbyId].UsersQueue.Remove(uid);

            lock (MemberTracker) MemberTracker.Remove(uid);

            return Task.FromResult(true);
        }

        public Task<bool> BanMember(int lobbyId, int uid, int adminUid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            if (Lobby[lobbyId].AdminUid != adminUid) return Task.FromResult(false);

            lock (Lobby) Lobby[lobbyId].BannedUsers.Add(uid);

            lock (MemberTracker) MemberTracker.Remove(uid);

            return Task.FromResult(true);
        }

        public Task<bool> UnbanMember(int lobbyId, int uid)
        {
            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            lock (Lobby) Lobby[lobbyId].BannedUsers.Remove(uid);

            return Task.FromResult(true);
        }
        public Task<bool> StartCheck(int lobbyId, int adminUid)
        {
            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            if (Lobby[lobbyId].AdminUid != adminUid) return Task.FromResult(false);

            lock (Lobby)
            {
                Lobby[lobbyId].LobbyReadyCheck = true;
                Lobby[lobbyId].UsersLobby.Where(u => u.Uid == adminUid).FirstOrDefault().Ready = true;
            }

            return Task.FromResult(true);
        }

        public Task<bool> CancelCheck(int lobbyId)
        {
            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            lock (Lobby)
            {
                Lobby[lobbyId].LobbyReadyCheck = false;
                Lobby[lobbyId].UsersLobby.ForEach(user =>
                {
                    user.Ready = false;
                });
            }
            return Task.FromResult(true);
        }

        public Task<bool> FinishLobby(int lobbyId)
        {
            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            Lobby[lobbyId].UsersLobby.ForEach(user =>
            {
                lock (MemberTracker) MemberTracker.Remove(user.Uid);
            });

            lock (Lobby) Lobby.Remove(lobbyId);

            return Task.FromResult(true);
        }

        public Task<bool> AcceptReady(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            lock (Lobby) Lobby[lobbyId].UsersLobby.Where(u => u.Uid == uid).FirstOrDefault().Ready = true;

            return Task.FromResult(true);
        }

        public Task<bool> DeclineReady(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            lock (Lobby) Lobby[lobbyId].UsersLobby.Where(u => u.Uid == uid).FirstOrDefault().Ready = false;

            return Task.FromResult(true);
        }

        public Task<bool> CheckIfAllReady(int lobbyId)
        {
            if (!Lobby.ContainsKey(lobbyId)) return Task.FromResult(false);

            var users = Lobby[lobbyId].UsersLobby.Select(u => u.Ready).ToList();

            foreach (var user in users)
            {
                if (!user) return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public Task<List<int>> GetMembersInLobby(int lobbyId)
        {
            return Task.FromResult(Lobby[lobbyId].UsersLobby.Select(user => user.Uid).ToList());
        }

        public Task<List<int>> GetMembersInQueueLobby(int lobbyId)
        {
            return Task.FromResult(Lobby[lobbyId].UsersQueue);
        }

        public Task<bool> CheckIfMemberInQueue(int lobbyId, int uid)
        {
            return Task.FromResult(Lobby[lobbyId].UsersQueue.Contains(uid));
        }

        public Task<bool> CheckIfMemberIsBanned(int lobbyId, int uid)
        {
            return Task.FromResult(Lobby[lobbyId].BannedUsers.Contains(uid));
        }
        public Task<bool> CheckIfMemberInAnyLobby(int uid)
        {
            return Task.FromResult(MemberTracker.ContainsKey(uid));
        }
        public Task<int> GetLobbyIdFromUser(int uid)
        {
            return Task.FromResult(MemberTracker[uid]);
        }
        public Task<int> GetLobbyAdmin(int lobbyId)
        {
            return Task.FromResult(Lobby[lobbyId].AdminUid);
        }
        public Task<bool> CheckIfMemberInLobby(int lobbyId, int uid)
        {
            return Task.FromResult(Lobby[lobbyId].UsersLobby.Select(user => user.Uid).Contains(uid));
        }
        public Task<bool> CheckIfLobbyExists(int lobbyId)
        {
            return Task.FromResult(Lobby.ContainsKey(lobbyId));
        }
        public Task<bool> CheckReadyState(int lobbyId)
        {
            return Task.FromResult(Lobby[lobbyId].LobbyReadyCheck);
        }
        public Task<bool> CheckIfLobbyFull(int lobbyId)
        {
            return Task.FromResult(Lobby[lobbyId].MaxUsers == Lobby[lobbyId].UsersLobby.Count);
        }
    }
}