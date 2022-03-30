import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LobbyHubService {

  hubUrl = environment.hubUrl;
  private hubConnection!: HubConnection;
  connectionStatus!: Promise<boolean>;

  acceptedMembers$ = new BehaviorSubject<number>(0);
  lobbyQueueMembers$ = new BehaviorSubject<number[]>([]);
  lobbyPartyMembers$ = new BehaviorSubject<number[]>([]);
  kickedPartyMembers$ = new BehaviorSubject<number>(0);
  kickedQueueMembers$ = new BehaviorSubject<number>(0);
  lobbyReadyCheck$ = new BehaviorSubject<boolean>(false);
  acceptedReadyCheckMembers$ = new BehaviorSubject<number[]>([]);
  declinedReadyCheckMembers$ = new BehaviorSubject<number>(0);
  lobbyStart$ = new BehaviorSubject<boolean>(false);

  constructor() {}

  getAcceptedMemberObserver(): Observable<number>{
    return this.acceptedMembers$.asObservable();
  }

  getLobbyQueueMembersObserver(): Observable<number[]>{
    return this.lobbyQueueMembers$.asObservable();
  }

  getLobbyPartyMembersObserver(): Observable<number[]>{
    return this.lobbyPartyMembers$.asObservable();
  }

  getLobbyKickedPartyMembersObserver(): Observable<number>{
    return this.kickedPartyMembers$.asObservable();
  }

  getLobbyKickedQueueMembersObserver(): Observable<number>{
    return this.kickedQueueMembers$.asObservable();
  }

  getLobbyReadyCheckObserver(): Observable<boolean>{
    return this.lobbyReadyCheck$.asObservable();
  }

  getAcceptedReadyCheckMembersObserver(): Observable<number[]>{
    return this.acceptedReadyCheckMembers$.asObservable();
  }

  getDeclinedReadyCheckMembersObserver(): Observable<number>{
    return this.declinedReadyCheckMembers$.asObservable();
  }

  getLobbyStartObserver(): Observable<boolean>{
    return this.lobbyStart$.asObservable();
  }




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
      this.hubConnection.on('Accepted', () => {
        console.log("You have been accepted!");
      });
      // Member that is declined will get this
      this.hubConnection.on('Declined', () => {
        console.log("You have been declined!");
      });
      // Member that is banned will get this
      this.hubConnection.on('Banned', () => {
        console.log("You have been declined!");
      });
      // Member that is kicked will get this
      this.hubConnection.on('Kicked', () => {
        console.log("You have been kicked!");
      });

      // Everyone in lobby will get this
      this.hubConnection.on("MemberAccepted", ids => {
        this.lobbyPartyMembers$.next(ids[1]);
        this.acceptedMembers$.next(ids[1]);
        console.log("User with id: " + ids[1] + " was accepted!");
      });

      this.hubConnection.on("MemberDeclined", ids => {
        this.kickedQueueMembers$.next(ids[1]);
        console.log("User with id: " + ids[1] + " was declined!");
      });

      this.hubConnection.on("MemberBanned", id => {
        console.log("User with id: " + id + " was banned!");
      });

      this.hubConnection.on("MemberKicked", ids => {
        this.kickedPartyMembers$.next(ids[1])
        console.log("User with id: " + ids[1] + " was kicked!");
      });

      this.hubConnection.on("JoinedLobbyQueue", id => {
        this.lobbyQueueMembers$.next(id);
        console.log("User with id: " + id + " joined lobby queue!");
      });

      this.hubConnection.on("LeftLobby", id => {
        this.kickedPartyMembers$.next(id);
        console.log("User with id: " + id + " left lobby party!");
      });

      this.hubConnection.on("LeftQueue", id => {
        this.kickedQueueMembers$.next(id);
        console.log("User with id: " + id + " left lobby queue!");
      });

      this.hubConnection.on("HostStarted", id => {
        this.acceptedReadyCheckMembers$.next(id);
        this.lobbyReadyCheck$.next(true);
        console.log("User with id: " + id + " started ready check!");
      });

      this.hubConnection.on("MemberDeclinedReady", id => {
        this.declinedReadyCheckMembers$.next(id);
        console.log("User with id: " + id + " declined ready check!");
      });

      this.hubConnection.on("MemberAcceptedReady", id => {
        this.acceptedReadyCheckMembers$.next(id);
        console.log("User with id: " + id + " accepted ready check!");
      });

      this.hubConnection.on("LobbyStarted", () => {
        this.lobbyStart$.next(true);
        console.log("Lobby starting...");
      });

      // Only caller will get this
      this.hubConnection.on("QueueMembers", ids => {
        ids.forEach((id: number[]) => {
          this.lobbyQueueMembers$.next(id);
        })
        console.log("Getting users in queue");
      });

      // Only caller will get this
      this.hubConnection.on("LobbyMembers", ids => {
        ids.forEach((id: number[]) => {
          this.lobbyPartyMembers$.next(id);
          console.log("Getting users in lobby");
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
    this.hubConnection.invoke("Accept", lobbyId);
  }
  async declineReadyCheck(lobbyId: number){
    await this.connectionStatus;
    this.hubConnection.invoke("Decline", lobbyId);
  }


  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }

  addUserToObservable(id: any) {
    this.lobbyQueueMembers$.next(id);
  }
}
