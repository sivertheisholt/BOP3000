using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities.Lobbies;
using API.Entities.Lobbies.LobbyTracking;

namespace API.SignalR
{
    public class LobbyChatTracker
    {

        private static readonly Dictionary<int, LobbyChatStatusTracker> Chat = new Dictionary<int, LobbyChatStatusTracker>();
        private static readonly Dictionary<int, int> MemberTracker = new Dictionary<int, int>();

        public void CreateChat(int lobbyId)
        {
            Console.WriteLine(lobbyId);
            var chat = new LobbyChatStatusTracker
            {
                LobbyId = lobbyId,
                Messages = new List<Message>(),
                Users = new List<int>()
            };
            lock (Chat) Chat.Add(lobbyId, chat);
        }

        public bool MemberJoinedChat(int lobbyId, int uid)
        {
            if (!Chat.ContainsKey(lobbyId)) return false;

            if (MemberTracker.ContainsKey(uid)) return false;

            lock (Chat) Chat[lobbyId].Users.Add(uid);

            lock (MemberTracker) MemberTracker.Add(uid, lobbyId);

            return true;
        }

        public bool SendMessage(int lobbyId, int uid, Message message)
        {
            Console.WriteLine(lobbyId);

            if (!Chat.ContainsKey(lobbyId)) return false;

            if (!Chat[lobbyId].Users.Contains(uid)) return false;

            lock (Chat) Chat[lobbyId].Messages.Add(message);

            return true;
        }

        public List<Message> GetMessages(int lobbyId)
        {
            return Chat[lobbyId].Messages.OrderBy(o => o.DateSent).ToList();
        }

        public void LobbyChatDone(int lobbyId)
        {
            foreach (var member in Chat[lobbyId].Users)
            {
                lock (MemberTracker) MemberTracker.Remove(member);
            }
            lock (Chat) Chat.Remove(lobbyId);
        }

        public int GetLobbyIdFromUser(int uid)
        {
            return MemberTracker.FirstOrDefault(member => member.Key == uid).Value;
        }
        public bool CheckIfChatExists(int lobbyId)
        {
            return Chat.ContainsKey(lobbyId);
        }
        public bool CheckIfUserInChat(int uid)
        {
            return MemberTracker.ContainsKey(uid);
        }
    }
}