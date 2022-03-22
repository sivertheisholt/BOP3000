using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Lobbies;

namespace API.SignalR
{
    public class LobbyChatTracker
    {
        private static readonly Dictionary<int, Dictionary<int, List<Message>>> Chat =
            new Dictionary<int, Dictionary<int, List<Message>>>();

        private static readonly Dictionary<int, int> MemberTracker = new Dictionary<int, int>();

        public Task CreateChat(int lobbyId)
        {
            lock (Chat)
            {
                Chat.Add(lobbyId, null);
            }
            return Task.CompletedTask;
        }

        public Task<bool> MemberJoinedChat(int lobbyId, int uid)
        {
            lock (Chat)
            {
                if (!Chat.ContainsKey(lobbyId)) return Task.FromResult(false);
                {
                    Chat[lobbyId].Add(uid, null);
                    MemberTracker.Add(uid, lobbyId);
                }
            }
            return Task.FromResult(true);
        }

        public Task<bool> SendMessage(int lobbyId, int uid, Message message)
        {
            lock (Chat)
            {
                if (!Chat.ContainsKey(lobbyId)) return Task.FromResult(false);
                if (!Chat[lobbyId].ContainsKey(uid)) return Task.FromResult(false);

                Chat[lobbyId][uid].Add(message);
            }
            return Task.FromResult(true);
        }

        public Task<List<Message>> GetMessages(int lobbyId)
        {
            List<Message> messages = new List<Message>();
            lock (Chat)
            {
                foreach (KeyValuePair<int, int> entry in MemberTracker)
                {
                    foreach (var message in Chat[lobbyId][entry.Key])
                    {
                        messages.Add(message);
                    }
                }
            }
            List<Message> SortedList = messages.OrderBy(o => o.DateSent).ToList();
            return Task.FromResult(SortedList);
        }

        public Task<int> GetLobbyIdFromUser(int uid)
        {
            return Task.FromResult(MemberTracker.Where(member => member.Key == uid).FirstOrDefault().Value);
        }
    }
}