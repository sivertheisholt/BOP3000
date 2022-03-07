using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.IRepositories;

namespace API.SignalR
{
    public class LobbyTracker
    {
        private static readonly Dictionary<int, List<int>> LobbiesQueue = new Dictionary<int, List<int>>();
        private static readonly Dictionary<int, List<int>> Lobbies = new Dictionary<int, List<int>>();
        private static readonly Dictionary<int, int> MemberTracker = new Dictionary<int, int>();
        private static readonly Dictionary<int, int> LobbiesAdminTracker = new Dictionary<int, int>();

        public LobbyTracker()
        {
        }

        public Task CreateLobby(int lobbyId, int adminUid)
        {
            lock (LobbiesQueue)
            {
                lock (Lobbies)
                {
                    lock (LobbiesAdminTracker)
                    {
                        LobbiesQueue.Add(lobbyId, new List<int>());
                        Lobbies.Add(lobbyId, new List<int>());
                        LobbiesAdminTracker.Add(lobbyId, adminUid);
                    }
                }
            }
            return Task.CompletedTask;
        }

        public Task<bool> JoinQueue(int lobbyId, int uid)
        {
            lock (LobbiesQueue)
            {
                if (MemberTracker.ContainsKey(uid))
                {
                    Console.WriteLine("User is already in another queue or lobby!");
                    return Task.FromResult(false);
                }

                if (Lobbies.ContainsKey(lobbyId))
                {
                    LobbiesQueue[lobbyId].Add(uid);
                    lock (MemberTracker)
                    {
                        MemberTracker.Add(uid, lobbyId);
                    }
                }
            }

            return Task.FromResult(true);
        }
        public Task<bool> AcceptMember(int lobbyId, int uid)
        {
            lock (Lobbies)
            {
                if (MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

                if (Lobbies.ContainsKey(lobbyId))
                {
                    if (Lobbies.Where(lobby => lobby.Key == lobbyId).FirstOrDefault().Value.Contains(uid)) Task.FromResult(false);

                    Lobbies[lobbyId].Add(uid);
                    lock (LobbiesQueue)
                    {
                        LobbiesQueue[lobbyId].Remove(uid);
                    }
                }
            }

            return Task.FromResult(true);
        }

        public Task MemberLeftLobby(int lobbyId, int uid)
        {
            lock (Lobbies)
            {
                if (MemberTracker.ContainsKey(uid))
                {
                    Lobbies[lobbyId].Remove(uid);
                    lock (MemberTracker)
                    {
                        MemberTracker.Remove(uid);
                    }
                }
            }

            return Task.CompletedTask;
        }

        public Task MemberLeftQueueLobby(int lobbyId, int uid)
        {
            lock (LobbiesQueue)
            {
                if (MemberTracker.ContainsKey(uid))
                {
                    LobbiesQueue[lobbyId].Remove(uid);
                    lock (MemberTracker)
                    {
                        MemberTracker.Remove(uid);
                    }
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<int>> GetMembersInLobby(int lobbyId)
        {
            return Task.FromResult(Lobbies[lobbyId].ToList());
        }

        public Task<List<int>> GetMembersInQueueLobby(int lobbyId)
        {
            return Task.FromResult(LobbiesQueue[lobbyId].ToList());
        }

        public Task<bool> CheckIfMemberInQueue(int lobbyId, int uid)
        {
            return Task.FromResult(LobbiesQueue[lobbyId].Contains(uid));
        }

        public Task<int> GetLobbyIdFromUser(int uid)
        {
            return Task.FromResult(MemberTracker.Where(member => member.Key == uid).FirstOrDefault().Value);
        }
        public Task<int> GetLobbyAdmin(int lobbyId)
        {
            return Task.FromResult(LobbiesAdminTracker[lobbyId]);
        }

        public Task<bool> CheckIfMemberInLobby(int lobbyId, int uid)
        {
            return Task.FromResult(Lobbies[lobbyId].Contains(uid));
        }
        public Task<bool> CheckIfLobbyExists(int lobbyId)
        {
            return Task.FromResult(Lobbies.ContainsKey(lobbyId));
        }
    }
}