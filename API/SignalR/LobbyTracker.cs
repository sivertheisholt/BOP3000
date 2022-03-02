using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class LobbyTracker
    {
        private static readonly Dictionary<int, List<int>> LobbiesQueue = new Dictionary<int, List<int>>();
        private static readonly Dictionary<int, List<int>> Lobbies = new Dictionary<int, List<int>>();
        private static readonly Dictionary<int, int> MemberTracker = new Dictionary<int, int>();

        public LobbyTracker()
        {
            CreateLobby(1);
        }

        public Task CreateLobby(int lobbyId)
        {
            LobbiesQueue.Add(lobbyId, new List<int>());
            Lobbies.Add(lobbyId, new List<int>());
            return Task.CompletedTask;
        }

        public Task MemberJoinedQueue(int lobbyId, int uid)
        {
            lock (LobbiesQueue)
            {
                if (MemberTracker.ContainsKey(uid)) return Task.FromException(new Exception("Member is already in a lobby queue"));

                if (Lobbies.ContainsKey(lobbyId))
                {
                    if (Lobbies.Where(lobby => lobby.Key == lobbyId).FirstOrDefault().Value.Contains(uid)) return Task.FromException(new Exception("Member already exists in lobby"));

                    if (LobbiesQueue.Where(lobby => lobby.Key == lobbyId).FirstOrDefault().Value.Contains(uid)) return Task.FromException(new Exception("Member already exists in lobby queue"));

                    LobbiesQueue[lobbyId].Add(uid);
                    MemberTracker.Add(uid, lobbyId);
                }
            }

            return Task.CompletedTask;
        }
        public Task MemberAccepted(int lobbyId, int uid)
        {
            lock (Lobbies)
            {
                if (MemberTracker.ContainsKey(uid)) return Task.FromException(new Exception("User is already in a lobby"));

                if (Lobbies.ContainsKey(lobbyId))
                {
                    if (Lobbies.Where(lobby => lobby.Key == lobbyId).FirstOrDefault().Value.Contains(uid)) return Task.FromException(new Exception("User already exists"));

                    Lobbies[lobbyId].Add(uid);
                    LobbiesQueue[lobbyId].Remove(uid);
                }
            }

            return Task.CompletedTask;
        }

        public Task MemberLeftLobby(int lobbyId, int uid)
        {
            lock (Lobbies)
            {
                if (MemberTracker.ContainsKey(uid))
                {
                    Lobbies[lobbyId].Remove(uid);
                    MemberTracker.Remove(uid);
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
                    MemberTracker.Remove(uid);
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
    }
}