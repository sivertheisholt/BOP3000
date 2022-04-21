import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root'
})
export class LobbyHubService {

  hubUrl = environment.hubUrl;
  private hubConnection!: HubConnection;
  connectionStatus!: Promise<boolean>;

  acceptedMembers = new Subject<number>();
  lobbyQueueMembers = new Subject<number[]>();
  lobbyPartyMembers = new Subject<number[]>();
  kickedPartyMembers = new Subject<number>();
  kickedQueueMembers = new Subject<number>();
  lobbyReadyCheck = new BehaviorSubject<boolean>(false);
  acceptedReadyCheckMembers = new Subject<number[]>();
  declinedReadyCheckMembers = new Subject<number>();
  lobbyStart = new BehaviorSubject<string>('');
  inQueue = new BehaviorSubject<string>('notInQueue');

  acceptedMembers$ = this.acceptedMembers.asObservable();
  lobbyQueueMembers$ = this.lobbyQueueMembers.asObservable();
  lobbyPartyMembers$ = this.lobbyPartyMembers.asObservable();
  kickedPartyMembers$ = this.kickedPartyMembers.asObservable();
  kickedQueueMembers$ = this.kickedQueueMembers.asObservable();
  lobbyReadyCheck$ = this.lobbyReadyCheck.asObservable();
  acceptedReadyCheckMembers$ = this.acceptedReadyCheckMembers.asObservable();
  declinedReadyCheckMembers$ = this.declinedReadyCheckMembers.asObservable();
  lobbyStart$ = this.lobbyStart.asObservable();
  inQueue$ = this.inQueue.asObservable();

  constructor(private router: Router, private notificationService: NotificationService) {}

  createHubConnection(token: string) {
    this.connectionStatus = new Promise(resolve => {
      this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'lobby', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build()

      this.hubConnection
        .start()
        .then(() => resolve(true))
        .catch(error => console.log(error));
    })


      
      // Member that is accepted will get this
      this.hubConnection.on('Accepted', id => {
        this.inQueue.next('accepted');
        this.notificationService.setNewNotification({type: 'success', message: 'You have been accepted to the lobby. Check your notifications to go directly to the lobby.', lobbyId: id});
      });
      // Member that is declined will get this
      this.hubConnection.on('Declined', id => {
        this.inQueue.next('notInQueue');
        this.notificationService.setNewNotification({type: 'info', message: 'You have been declined from the lobby.'});
      });
      // Member that is banned will get this
      this.hubConnection.on('Banned', id => {
        this.inQueue.next('notInQueue');
        this.redirectUser(this.router.url, +id);
        this.notificationService.setNewNotification({type: 'error', message: 'You have been banned from the lobby.'});
      });
      // Member that is kicked will get this
      this.hubConnection.on('Kicked', id => {
        this.inQueue.next('notInQueue');
        this.redirectUser(this.router.url, +id);
        this.notificationService.setNewNotification({type: 'error', message: 'You have been kicked from the lobby.'});
      });
      this.hubConnection.on('InQueue', () => {
        this.inQueue.next('inQueue');
        this.notificationService.setNewNotification({type: 'info', message: 'You have requested to join a lobby.'});
      });

      this.hubConnection.on("NotInDiscordServer", () => {
        console.log("Not in discord server");
      });

      // Everyone in lobby will get this
      this.hubConnection.on("MemberAccepted", ids => {
        this.lobbyPartyMembers.next(ids[1]);
        this.acceptedMembers.next(ids[1]);
        console.log("User with id: " + ids[1] + " was accepted!");
      });

      this.hubConnection.on("MemberDeclined", ids => {
        this.kickedQueueMembers.next(ids[1]);
        console.log("User with id: " + ids[1] + " was declined!");
      });

      this.hubConnection.on("MemberBanned", id => {
        console.log("User with id: " + id + " was banned!");
      });

      this.hubConnection.on("MemberKicked", ids => {
        this.kickedPartyMembers.next(ids[1])
        console.log("User with id: " + ids[1] + " was kicked!");
      });

      this.hubConnection.on("JoinedLobbyQueue", id => {
        this.lobbyQueueMembers.next(id);
        console.log("User with id: " + id + " joined lobby queue!");
      });

      this.hubConnection.on("LeftLobby", id => {
        this.kickedPartyMembers.next(id);
        console.log("User with id: " + id + " left lobby party!");
      });

      this.hubConnection.on("LeftQueue", id => {
        this.kickedQueueMembers.next(id);
        console.log("User with id: " + id + " left lobby queue!");
      });

      this.hubConnection.on("HostStarted", id => {
        this.acceptedReadyCheckMembers.next(id);
        this.lobbyReadyCheck.next(true);
        console.log("User with id: " + id + " started ready check!");
      });

      this.hubConnection.on("MemberDeclinedReady", id => {
        this.declinedReadyCheckMembers.next(id);
        console.log("User with id: " + id + " declined ready check!");
      });

      this.hubConnection.on("MemberAcceptedReady", id => {
        this.acceptedReadyCheckMembers.next(id);
        console.log("User with id: " + id + " accepted ready check!");
      });
      
      this.hubConnection.on("FullLobby", () => {
        this.notificationService.setNewNotification({type: 'error', message: "Can't accept new members, lobby is full"});
      });

      this.hubConnection.on("EndedLobby", id => {
        this.notificationService.setNewNotification({type: 'error', message: "Lobby was ended by the host."});
        this.redirectUser(this.router.url, id);
      });

      this.hubConnection.on("LobbyStarted", url => {
        this.lobbyStart.next(url);
      });

      this.hubConnection.on("QueueKicked", () => {
        this.notificationService.setNewNotification({type: 'info', message: 'Left queue as lobby has started.'});
      });
      
      this.hubConnection.on("LobbyCancelled", () => {
        console.log("Ready check cancelled...");
      });

      this.hubConnection.on("RedirectFinished", id => {
        this.redirectUserAfterFinished(this.router.url, id);
      });

      // Only caller will get this
      this.hubConnection.on("QueueMembers", ids => {
        console.log("Getting users in queue");
        ids.forEach((id: number[]) => {
          this.lobbyQueueMembers.next(id);
        });
      });

      // Only caller will get this
      this.hubConnection.on("LobbyMembers", ids => {
        console.log("Getting users in lobby");
        ids.forEach((id: number[]) => {
          this.lobbyPartyMembers.next(id);
        });
      });
  }

  async goInQueue(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("JoinQueue", lobbyId);
  }
  async getQueueMembers(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("GetQueueMembers", lobbyId);
  }
  async getLobbyMembers(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("GetLobbyMembers", lobbyId);
  }
  async acceptMemberFromQueue(lobbyId: number, uid: number){
    await this.connectionStatus;
    this.hubConnection.invoke("AcceptMember", lobbyId, +uid);
  }
  async declineMemberFromQueue(lobbyId: number, uid: number){
    await this.connectionStatus;
    this.hubConnection.invoke("DeclineMember", lobbyId, +uid);
  }
  async banMemberFromQueue(lobbyId: number, uid: number){
    await this.connectionStatus;
    this.hubConnection.invoke("BanMember", lobbyId, +uid);
  }
  async kickMemberFromParty(lobbyId: number, uid: number){
    await this.connectionStatus;
    this.hubConnection.invoke("KickMember", lobbyId, +uid);
  }
  async leaveQueue(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("LeaveQueue", lobbyId);
  }
  async leaveParty(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("LeaveLobby", lobbyId);
  }
  async startReadyCheck(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("StartCheck", lobbyId);
  }
  async acceptReadyCheck(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("AcceptCheck", lobbyId);
  }
  async declineReadyCheck(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("DeclineCheck", lobbyId);
  }
  async endLobby(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("EndLobby", lobbyId);
  }

  


  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }

  addUserToObservable(id: any) {
    this.lobbyQueueMembers.next(id);
  }

  redirectUser(currentUrl: string, lobbyId: number){
    const lobbyUrl: string = '/lobby/' + lobbyId;
    if(currentUrl == lobbyUrl){
      this.router.navigate(['..']);
    }
    return;
  }

  redirectUserAfterFinished(currentUrl: string, lobbyId: number){
    const lobbyUrl: string = '/lobby/' + lobbyId;
    if(currentUrl == lobbyUrl){
      this.router.navigate(['archived-lobby/' + lobbyId]);
    }
    return;
  }
}
