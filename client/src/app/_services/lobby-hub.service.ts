import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { QueueStatus } from '../_models/queuestatus.model';
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
  lobbyReadyCheck = new Subject<boolean>();
  acceptedReadyCheckMembers = new Subject<number[]>();
  declinedReadyCheckMembers = new Subject<number>();
  lobbyStart = new BehaviorSubject<string>('');
  inQueue = new BehaviorSubject<QueueStatus>({lobbyId: 0, inQueue: false, inQueueStatus: ''});

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
        this.inQueue.next({lobbyId: id, inQueue: false, inQueueStatus: 'accepted'});
        this.acceptedResponse(id);
        this.notificationService.setNewNotification({type: 'success', message: 'You have been accepted to the lobby. Check your notifications to go directly to the lobby.', lobbyId: id});
      });

      // Member that is declined will get this
      this.hubConnection.on('Declined', id => {
        this.inQueue.next({lobbyId: 0, inQueue: false, inQueueStatus: 'notInQueue'});
        this.notificationService.setNewNotification({type: 'info', message: 'You have been declined from the lobby.'});
      });

      // Member that is banned will get this
      this.hubConnection.on('Banned', id => {
        this.inQueue.next({lobbyId: 0, inQueue: false, inQueueStatus: 'notInQueue'});
        this.redirectUser(this.router.url, +id);
        this.notificationService.setNewNotification({type: 'error', message: 'You have been banned from the lobby.'});
      });

      // Member that is kicked will get this
      this.hubConnection.on('Kicked', id => {
        this.inQueue.next({lobbyId: 0, inQueue: false, inQueueStatus: 'notInQueue'});
        this.kickedResponse(id);
        this.redirectUser(this.router.url, +id);
        this.notificationService.setNewNotification({type: 'error', message: 'You have been kicked from the lobby.'});
      });

      this.hubConnection.on('InQueue', id => {
        this.inQueue.next({lobbyId: id, inQueue: true, inQueueStatus: 'inQueue'});
        this.notificationService.setNewNotification({type: 'success', message: 'You have requested to join a lobby.'});
      });

      this.hubConnection.on("NotInDiscordServer", () => {
        this.inQueue.next({lobbyId: 0, inQueue: false, inQueueStatus: 'notInQueue'});
        this.notificationService.setNewNotification({type: 'info', message: 'You need to join our Discord server in order to join lobbies. Check your notifications to join our Discord.', inDiscordServer: true});
      });

      this.hubConnection.on("CancelQueue", () => {
        this.inQueue.next({lobbyId: 0, inQueue: false, inQueueStatus: 'notInQueue'});
        this.notificationService.setNewNotification({type: 'info', message: 'You left the queue.'});
      });

      this.hubConnection.on("CancelLobby", () => {
        this.inQueue.next({lobbyId: 0, inQueue: false, inQueueStatus: 'notInQueue'});
        this.notificationService.setNewNotification({type: 'info', message: 'You left the lobby.'});
      });

      this.hubConnection.on("ServerError", message => {
        this.notificationService.setNewNotification({type: 'error', message: message});
      });

      // Everyone in lobby will get this
      this.hubConnection.on("MemberAccepted", ids => {
        this.lobbyPartyMembers.next(ids[1]);
        this.acceptedMembers.next(ids[1]);
      });

      this.hubConnection.on("MemberDeclined", ids => {
        this.kickedQueueMembers.next(ids[1]);
      });

      this.hubConnection.on("MemberBanned", id => {
        console.log("User with id: " + id + " was banned!");
      });

      this.hubConnection.on("MemberKicked", ids => {
        this.kickedPartyMembers.next(ids[1])
      });

      this.hubConnection.on("JoinedLobbyQueue", id => {
        this.lobbyQueueMembers.next(id);
      });

      this.hubConnection.on("LeftLobby", id => {
        this.kickedPartyMembers.next(id);
      });

      this.hubConnection.on("LeftQueue", id => {
        this.kickedQueueMembers.next(id);
      });

      this.hubConnection.on("HostStarted", id => {
        const lobbyUrl: string = '/lobby/' + +id[1];
        if(this.router.url != lobbyUrl){
          this.router.navigate(['lobby/', +id[1]]);
        }
        this.acceptedReadyCheckMembers.next([id[0]]);
        this.lobbyReadyCheck.next(true);
      });

      this.hubConnection.on("MemberDeclinedReady", id => {
        this.lobbyReadyCheck.next(false);
        this.acceptedReadyCheckMembers.next([]);
        this.declinedReadyCheckMembers.next(id);
      });

      this.hubConnection.on("MemberAcceptedReady", id => {
        this.acceptedReadyCheckMembers.next(id);
      });
      
      this.hubConnection.on("FullLobby", () => {
        this.notificationService.setNewNotification({type: 'error', message: "Can't accept new members, lobby is full"});
      });

      this.hubConnection.on("EndedLobby", id => {
        this.inQueue.next({lobbyId: 0, inQueue: false, inQueueStatus: 'notInQueue'});
        this.redirectUser(this.router.url, +id);
        this.notificationService.setNewNotification({type: 'error', message: "Lobby was ended by the host."});
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
        const finishedLobbyUrl = '/archived-lobby/' + id;
        const lobbyUrl: string = '/lobby/' + id;
        if(this.router.url == lobbyUrl){
          window.location.href = finishedLobbyUrl;
        }
        this.inQueue.next({lobbyId: 0, inQueue: false, inQueueStatus: 'notInQueue'});
      });

      // Only caller will get this
      this.hubConnection.on("QueueMembers", ids => {
        ids.forEach((id: number[]) => {
          this.lobbyQueueMembers.next(id);
        });
      });

      // Only caller will get this
      this.hubConnection.on("LobbyMembers", ids => {
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
  async acceptedResponse(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("AcceptedResponse", lobbyId);
  }
  async kickedResponse(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("KickedResponse", lobbyId);
  }
  async createdLobby(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("CreatedLobby", lobbyId);
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
}
