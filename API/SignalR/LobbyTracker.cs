using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Interfaces.IRepositories;

namespace API.SignalR
{
    public class LobbyTracker
    {

        // <lobbyId, uid's>
        private static readonly Dictionary<int, List<int>> LobbiesQueue = new Dictionary<int, List<int>>();

        // <lobbyId, uid's>
        private static readonly Dictionary<int, List<int>> Lobbies = new Dictionary<int, List<int>>();

        // <lobbyId, uid's>
        private static readonly Dictionary<int, int> LobbiesAdminTracker = new Dictionary<int, int>();

        // <lobbyId, uid's>
        private static readonly Dictionary<int, List<int>> BannedMembers = new Dictionary<int, List<int>>();

        // <uid, lobbyId>
        private static readonly Dictionary<int, int> MemberTracker = new Dictionary<int, int>();


        // <lobbyId, true/false>
        private static readonly Dictionary<int, bool> LobbyReadyCheck = new Dictionary<int, bool>();

        // <lobbyId, >
        private static readonly Dictionary<int, Dictionary<int, bool>> LobbyUserCheck = new Dictionary<int, Dictionary<int, bool>>();

        public LobbyTracker()
        {
        }

        public Task CreateLobby(int lobbyId, int adminUid)
        {
            lock (LobbiesQueue)
            {
                LobbiesQueue.Add(lobbyId, new List<int>());
                lock (Lobbies)
                {
                    Lobbies.Add(lobbyId, new List<int>());
                    Lobbies[lobbyId].Add(adminUid);
                    lock (LobbiesAdminTracker)
                    {
                        LobbiesAdminTracker.Add(lobbyId, adminUid);
                        lock (MemberTracker)
                        {
                            MemberTracker.Add(adminUid, lobbyId);
                            lock (BannedMembers)
                            {
                                BannedMembers.Add(lobbyId, new List<int>());
                                lock (LobbyReadyCheck)
                                {
                                    LobbyReadyCheck.Add(lobbyId, false);
                                    lock (LobbyUserCheck)
                                    {
                                        LobbyUserCheck.Add(lobbyId, new Dictionary<int, bool>());
                                        LobbyUserCheck[lobbyId].Add(adminUid, false);
                                    }
                                }
                            }
                        }
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
                if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);

                if (Lobbies.ContainsKey(lobbyId))
                {
                    if (!LobbiesQueue[lobbyId].Contains(uid)) return Task.FromResult(false);

                    Lobbies[lobbyId].Add(uid);
                    lock (LobbiesQueue)
                    {
                        LobbiesQueue[lobbyId].Remove(uid);
                        lock (LobbyUserCheck)
                        {
                            LobbyUserCheck[lobbyId].Add(uid, false);
                        }
                    }
                }
            }

            return Task.FromResult(true);
        }

        internal Task<bool> KickMember(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);
            lock (Lobbies)
            {
                Lobbies[lobbyId].Remove(uid);
                lock (MemberTracker)
                {
                    MemberTracker.Remove(uid);
                    lock (LobbyUserCheck)
                    {
                        LobbyUserCheck[lobbyId].Remove(uid);
                    }
                }
            }
            return Task.FromResult(true);
        }

        internal Task<bool> CheckIfMemberIsBanned(int lobbyId, int uid)
        {
            return Task.FromResult(BannedMembers[lobbyId].Contains(uid));
        }

        public Task<bool> DeclineMember(int lobbyId, int uid)
        {
            if (!MemberTracker.ContainsKey(uid)) return Task.FromResult(false);
            lock (LobbiesQueue)
            {
                LobbiesQueue[lobbyId].Remove(uid);
                lock (MemberTracker)
                {
                    MemberTracker.Remove(uid);
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

        public Task<bool> BanMember(int lobbyId, int uid)
        {
            lock (BannedMembers)
            {
                BannedMembers[lobbyId].Add(uid);
                lock (MemberTracker)
                {
                    MemberTracker.Remove(uid);
                }
            }
            return Task.FromResult(true);
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

        public Task<bool> CheckIfMemberInAnyLobby(int uid)
        {
            return Task.FromResult(MemberTracker.ContainsKey(uid));
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
        public Task StartCheck(int lobbyId)
        {
            lock (LobbyReadyCheck)
            {
                LobbyReadyCheck[lobbyId] = true;
            }
            return Task.CompletedTask;
        }
        public Task<bool> CheckReadyState(int lobbyId)
        {
            return Task.FromResult(LobbyReadyCheck[lobbyId]);
        }

        public Task FinishLobby(int lobbyId)
        {
            Lobbies[lobbyId].ForEach(uid =>
            {
                lock (MemberTracker) MemberTracker.Remove(uid);
                lock (BannedMembers) BannedMembers[lobbyId].Remove(uid);
            });

            lock (LobbiesQueue) LobbiesQueue.Remove(lobbyId);
            lock (Lobbies) Lobbies.Remove(lobbyId);
            lock (LobbiesAdminTracker) LobbiesAdminTracker.Remove(lobbyId);
            lock (LobbyReadyCheck) LobbyReadyCheck.Remove(lobbyId);
            lock (LobbyUserCheck) LobbyUserCheck.Remove(lobbyId);

            return Task.CompletedTask;
        }
        public Task AcceptReady(int lobbyId, int uid)
        {
            lock (LobbyUserCheck) LobbyUserCheck[lobbyId].Add(uid, true);
            return Task.CompletedTask;
        }
        public Task DeclineReady(int lobbyId, int uid)
        {
            lock (LobbyUserCheck) LobbyUserCheck[lobbyId].Add(uid, false);
            return Task.CompletedTask;
        }
    }
}