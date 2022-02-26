using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extentions;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class LobbyHub : Hub
    {
        private readonly LobbyTracker _lobbyTracker;
        public LobbyHub(LobbyTracker lobbyTracker)
        {
            _lobbyTracker = lobbyTracker;
        }

        private async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            //await Clients.Group(groupName).SendAsync("JoinedLobbyQueue", $"{Context.ConnectionId} has joined the group {groupName}.");
        }

        private async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);

            //await Clients.Group(groupName).SendAsync("Send", $"{Context.ConnectionId} has left the group {groupName}.");
        }

        public async Task OnMemberAccepted()
        {
            var httpContext = Context.GetHttpContext();
            var lobbyId = Int32.Parse(httpContext.Request.Query["lobbyId"]);
            try
            {
                await _lobbyTracker.MemberAccepted(lobbyId, Context.User.GetUserId());
            }
            catch (System.Exception)
            {
                //Return failed here to frontend
            }

            var groupNameQueue = lobbyId + "-Queue";

            var users = await _lobbyTracker.GetUsersInLobby(lobbyId);
            await Clients.Group(groupNameQueue).SendAsync("MemberAccepted", Context.User.GetUserId());
            await RemoveFromGroup(groupNameQueue);
            await AddToGroup(lobbyId.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var lobbyId = Int32.Parse(httpContext.Request.Query["lobbyId"]);
            try
            {
                await _lobbyTracker.MemberJoinedQueue(lobbyId, Context.User.GetUserId());
            }
            catch (System.Exception)
            {
                //Return failed here to frontend
            }
            var groupNameQueue = lobbyId + "-Queue";
            await AddToGroup(groupNameQueue);
            await Clients.Group(lobbyId.ToString()).SendAsync("JoinedLobbyQueue", Context.User.GetUserId());
            await Clients.Group(groupNameQueue).SendAsync("JoinedLobbyQueue", Context.User.GetUserId());
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var httpContext = Context.GetHttpContext();
            var lobbyId = Int32.Parse(httpContext.Request.Query["lobbyId"]);

            var groupNameQueue = lobbyId + "-Queue";
            await Clients.Group(groupNameQueue).SendAsync("LeftQueue", Context.User.GetUserId());

            await base.OnDisconnectedAsync(exception);
        }
    }
}