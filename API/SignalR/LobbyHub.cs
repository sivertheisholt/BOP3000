using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extentions;
using API.Interfaces.IRepositories;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class LobbyHub : Hub
    {
        private readonly LobbyTracker _lobbyTracker;

        private readonly ILobbiesRepository _lobbiesRepository;
        public LobbyHub(LobbyTracker lobbyTracker, ILobbiesRepository lobbiesRepository)
        {
            _lobbiesRepository = lobbiesRepository;
            _lobbyTracker = lobbyTracker;
        }

        private async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        private async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task OnMemberAccepted()
        {
            var httpContext = Context.GetHttpContext();
            var lobbyId = Int32.Parse(httpContext.Request.Query["lobbyId"]);
            var adminUid = await _lobbyTracker.GetLobbyAdmin(lobbyId);

            if (adminUid != Context.User.GetUserId()) return;

            try
            {
                await _lobbyTracker.MemberAccepted(lobbyId, Context.User.GetUserId());
            }
            catch (System.Exception)
            {
                //Return failed here to frontend
            }

            var groupNameQueue = lobbyId + "-Queue";

            await Clients.Group(groupNameQueue).SendAsync("MemberAccepted", Context.User.GetUserId());
            await RemoveFromGroup(groupNameQueue);
            await AddToGroup(lobbyId.ToString());
            await Clients.Group(lobbyId.ToString()).SendAsync("MemberAccepted", Context.User.GetUserId());
        }

        public override async Task OnConnectedAsync()
        {

            if (!await _lobbyTracker.CheckIfMemberInQueue(1, 1))
            {
                if (!await _lobbyTracker.CheckIfMemberInLobby(1, 1))
                {
                    await _lobbyTracker.CreateLobby(1, 1);
                }
            }
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var lobbyId = await _lobbyTracker.GetLobbyIdFromUser(Context.User.GetUserId());
            if (lobbyId == 0) return;

            var groupNameQueue = lobbyId + "-Queue";

            if (await _lobbyTracker.CheckIfMemberInQueue(lobbyId, Context.User.GetUserId()))
            {
                await _lobbyTracker.MemberLeftQueueLobby(lobbyId, Context.User.GetUserId());
                await Clients.Group(groupNameQueue).SendAsync("LeftLobbyQueue", Context.User.GetUserId());
                await Clients.Group(lobbyId.ToString()).SendAsync("LeftLobbyQueue", Context.User.GetUserId());
            }
            else
            {
                await _lobbyTracker.MemberLeftLobby(lobbyId, Context.User.GetUserId());
                await Clients.Group(groupNameQueue).SendAsync("LeftLobby", Context.User.GetUserId());
                await Clients.Group(lobbyId.ToString()).SendAsync("LeftLobby", Context.User.GetUserId());
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task OnQueuePending(int lobbyId)
        {
            var httpContext = Context.GetHttpContext();
            try
            {
                await _lobbyTracker.MemberJoinedQueue(lobbyId, Context.User.GetUserId());
            }
            catch (System.Exception err)
            {
                Console.WriteLine(err);
                await Clients.Caller.SendAsync("GetQueueMembers", await _lobbyTracker.GetMembersInQueueLobby(lobbyId));
                return;
            }
            var groupNameQueue = lobbyId + "-Queue";
            await AddToGroup(groupNameQueue);
            await Clients.OthersInGroup(lobbyId.ToString()).SendAsync("JoinedLobbyQueue", Context.User.GetUserId());
            await Clients.OthersInGroup(groupNameQueue).SendAsync("JoinedLobbyQueue", Context.User.GetUserId());
            await Clients.Caller.SendAsync("GetQueueMembers", await _lobbyTracker.GetMembersInQueueLobby(lobbyId));
        }
    }
}