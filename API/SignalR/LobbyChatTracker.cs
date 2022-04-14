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

        // <uid, lobbyId>
        private static readonly Dictionary<int, int> MemberTracker = new Dictionary<int, int>();

        public Task CreateChat(int lobbyId)
        {
            lock (Chat)
            {
                Chat.Add(lobbyId, new Dictionary<int, List<Message>>());
            }
            return Task.CompletedTask;
        }

        public Task<bool> MemberJoinedChat(int lobbyId, int uid)
        {
            lock (Chat)
            {
                if (!Chat.ContainsKey(lobbyId)) return Task.FromResult(false);

                Chat[lobbyId].Add(uid, new List<Message>());
                MemberTracker.Add(uid, lobbyId);
            }
            return Task.FromResult(true);
        }

        public Task<bool> SendMessage(int lobbyId, int uid, Message message)
        {
            lock (Chat)
            {
                if (!Chat.ContainsKey(lobbyId))
                {
                    Console.WriteLine("Lobby doesnt exist");
                    return Task.FromResult(false);
                }
                if (!Chat[lobbyId].ContainsKey(uid))
                {
                    Console.WriteLine("User doesnt exist");
                    return Task.FromResult(false);
                }

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

        public Task LobbyChatDone(int lobbyId)
        {
            foreach (var member in Chat[lobbyId])
            {
                lock (MemberTracker) MemberTracker.Remove(member.Key);
            }
            lock (Chat) Chat.Remove(lobbyId);
            return Task.CompletedTask;
        }

        public Task<int> GetLobbyIdFromUser(int uid)
        {
            return Task.FromResult(MemberTracker.Where(member => member.Key == uid).FirstOrDefault().Value);
        }
        public Task<bool> CheckIfChatExists(int lobbyId)
        {
            return Task.FromResult(Chat.ContainsKey(lobbyId));
        }
        public Task<bool> CheckIfUserInChat(int uid)
        {
            return Task.FromResult(MemberTracker.ContainsKey(uid));
        }
    }
}