using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs.Lobbies;
using API.Entities.Lobbies;
using API.Entities.Lobbies.LobbyTracking;
using API.Interfaces.IRepositories;

namespace API.SignalR
{
    public class LobbyTracker
    {
        private static readonly Dictionary<int, LobbyStatusTracker> Lobby = new Dictionary<int, LobbyStatusTracker>();
        private static readonly Dictionary<int, int> MemberTracker = new Dictionary<int, int>();

        public void CreateLobby(Lobby lobby, int adminUid)
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
        }

        public bool JoinQueue(int lobbyId, int uid)
        {
            if (MemberTracker.ContainsKey(uid)) return false;

            if (!Lobby.ContainsKey(lobbyId)) return false;

            lock (Lobby) Lobby[lobbyId].UsersQueue.Add(uid);
            lock (MemberTracker) MemberTracker.Add(uid, lobbyId);

            return true;
        }

        public bool AcceptMember(int lobbyId, int uid, int adminUid)
        {
            if (!MemberTracker.ContainsKey(uid)) return false;

            if (!Lobby.ContainsKey(lobbyId)) return false;

            if (!Lobby[lobbyId].UsersQueue.Contains(uid)) return false;

            if (Lobby[lobbyId].AdminUid != adminUid) return false;

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

            return true;
        }

        public bool DeclineMember(int lobbyId, int uid, int adminUid)
        {
            if (!MemberTracker.ContainsKey(uid)) return false;

            if (!Lobby.ContainsKey(lobbyId)) return false;

            if (Lobby[lobbyId].AdminUid != adminUid) return false;

            lock (Lobby) Lobby[lobbyId].UsersQueue.Remove(uid);

            lock (MemberTracker) MemberTracker.Remove(uid);

            return true;
        }

        public bool KickMember(int lobbyId, int uid, int adminUid)
        {
            if (!MemberTracker.ContainsKey(uid)) return false;

            if (!Lobby.ContainsKey(lobbyId)) return false;

            if (Lobby[lobbyId].AdminUid != adminUid) return false;

            var userLobby = Lobby[lobbyId].UsersLobby.FirstOrDefault(user => user.Uid == uid);

            lock (Lobby) Lobby[lobbyId].UsersLobby.Remove(userLobby);

            lock (MemberTracker) MemberTracker.Remove(uid);

            return true;
        }

        public bool MemberLeftLobby(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return false;

            if (!Lobby.ContainsKey(lobbyId)) return false;

            var userLobby = Lobby[lobbyId].UsersLobby.FirstOrDefault(user => user.Uid == uid);

            lock (Lobby) Lobby[lobbyId].UsersLobby.Remove(userLobby);
            lock (MemberTracker) MemberTracker.Remove(uid);

            return true;
        }

        public bool MemberLeftQueueLobby(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return false;

            if (!Lobby.ContainsKey(lobbyId)) return false;

            lock (Lobby) Lobby[lobbyId].UsersQueue.Remove(uid);

            lock (MemberTracker) MemberTracker.Remove(uid);

            return true;
        }

        public bool BanMember(int lobbyId, int uid, int adminUid)
        {
            if (!MemberTracker.ContainsKey(uid)) return false;

            if (!Lobby.ContainsKey(lobbyId)) return false;

            if (Lobby[lobbyId].AdminUid != adminUid) return false;

            lock (Lobby) Lobby[lobbyId].BannedUsers.Add(uid);

            lock (MemberTracker) MemberTracker.Remove(uid);

            return true;
        }

        public bool UnbanMember(int lobbyId, int uid)
        {
            if (!Lobby.ContainsKey(lobbyId)) return false;

            lock (Lobby) Lobby[lobbyId].BannedUsers.Remove(uid);

            return true;
        }
        public bool StartCheck(int lobbyId, int adminUid)
        {
            if (!Lobby.ContainsKey(lobbyId)) return false;

            if (Lobby[lobbyId].AdminUid != adminUid) return false;

            lock (Lobby)
            {
                Lobby[lobbyId].LobbyReadyCheck = true;
                Lobby[lobbyId].UsersLobby.FirstOrDefault(u => u.Uid == adminUid).Ready = true;
            }

            return true;
        }

        public bool CancelCheck(int lobbyId)
        {
            if (!Lobby.ContainsKey(lobbyId)) return false;

            lock (Lobby)
            {
                Lobby[lobbyId].LobbyReadyCheck = false;
                Lobby[lobbyId].UsersLobby.ForEach(user =>
                {
                    user.Ready = false;
                });
            }
            return true;
        }

        public void FinishLobby(int lobbyId)
        {
            Lobby[lobbyId].UsersLobby.ForEach(user =>
            {
                lock (MemberTracker) MemberTracker.Remove(user.Uid);
            });

            lock (Lobby) Lobby.Remove(lobbyId);
        }

        public bool AcceptReady(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return false;

            if (!Lobby.ContainsKey(lobbyId)) return false;

            lock (Lobby) Lobby[lobbyId].UsersLobby.FirstOrDefault(u => u.Uid == uid).Ready = true;

            return true;
        }

        public bool DeclineReady(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return false;

            if (!Lobby.ContainsKey(lobbyId)) return false;

            lock (Lobby) Lobby[lobbyId].UsersLobby.FirstOrDefault(u => u.Uid == uid).Ready = false;

            return true;
        }

        public bool CheckIfAllReady(int lobbyId)
        {
            if (!Lobby.ContainsKey(lobbyId)) return false;

            var users = Lobby[lobbyId].UsersLobby.Select(u => u.Ready).ToList();

            foreach (var user in users)
            {
                if (!user) return false;
            }

            return true;
        }

        public List<int> GetMembersInLobby(int lobbyId)
        {
            return Lobby[lobbyId].UsersLobby.Select(user => user.Uid).ToList();
        }

        public List<int> GetMembersInQueueLobby(int lobbyId)
        {
            return Lobby[lobbyId].UsersQueue;
        }

        public bool CheckIfMemberInQueue(int lobbyId, int uid)
        {
            return Lobby[lobbyId].UsersQueue.Contains(uid);
        }

        public bool CheckIfMemberIsBanned(int lobbyId, int uid)
        {
            return Lobby[lobbyId].BannedUsers.Contains(uid);
        }
        public bool CheckIfMemberInAnyLobby(int uid)
        {
            return MemberTracker.ContainsKey(uid);
        }
        public int GetLobbyIdFromUser(int uid)
        {
            return MemberTracker[uid];
        }
        public int GetLobbyAdmin(int lobbyId)
        {
            return Lobby[lobbyId].AdminUid;
        }
        public bool CheckIfMemberInLobby(int lobbyId, int uid)
        {
            return Lobby[lobbyId].UsersLobby.Select(user => user.Uid).Contains(uid);
        }
        public bool CheckIfLobbyExists(int lobbyId)
        {
            return Lobby.ContainsKey(lobbyId);
        }
        public bool CheckReadyState(int lobbyId)
        {
            return Lobby[lobbyId].LobbyReadyCheck;
        }
        public bool CheckIfLobbyFull(int lobbyId)
        {
            return Lobby[lobbyId].MaxUsers == Lobby[lobbyId].UsersLobby.Count;
        }
    }
}