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
            var chat = new LobbyChatStatusTracker
            {
                LobbyId = lobbyId,
                Messages = new List<Message>(),
                Users = new List<int>()
            };
            lock (Chat) Chat.Add(lobbyId, chat);
        }

        public void MemberJoinedChat(int lobbyId, int uid)
        {
            lock (Chat) Chat[lobbyId].Users.Add(uid);

            lock (MemberTracker) MemberTracker.Add(uid, lobbyId);
        }

        public void MemberLeftChat(int lobbyId, int uid)
        {
            lock (Chat) Chat[lobbyId].Users.Remove(uid);

            lock (MemberTracker) MemberTracker.Remove(uid);
        }

        public void SendMessage(int lobbyId, int uid, Message message)
        {

            lock (Chat) Chat[lobbyId].Messages.Add(message);
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
        public bool CheckIfMemberInAnyChat(int uid)
        {
            return MemberTracker.ContainsKey(uid);
        }
        public bool CheckIfMemberInChat(int lobbyId, int uid)
        {
            return Chat[lobbyId].Users.Contains(uid);
        }
    }
}