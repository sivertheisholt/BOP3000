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
                    Lobbies[lobbyId].RemoveAt(Lobbies[lobbyId].Where(list => list == uid).FirstOrDefault());
                }
            }

            return Task.CompletedTask;
        }

        public Task MemberLeftLobbyQueue(int lobbyId, int uid)
        {
            lock (LobbiesQueue)
            {
                if (MemberTracker.ContainsKey(uid))
                {
                    LobbiesQueue[lobbyId].RemoveAt(LobbiesQueue[lobbyId].Where(list => list == uid).FirstOrDefault());
                }
            }

            return Task.CompletedTask;
        }

        public Task<List<int>> GetUsersInLobby(int lobbyId)
        {
            return Task.FromResult(Lobbies[lobbyId].ToList());
        }

    }
}