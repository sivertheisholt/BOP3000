import { Injectable } from '@angular/core';
import { waitForAsync } from '@angular/core/testing';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class LobbyHubService {

  hubUrl = environment.hubUrl;
  private hubConnection!: HubConnection;
  connectionStatus!: Promise<boolean>;

  acceptedMembersSource = new BehaviorSubject<number>(0);
  acceptedMembers$ = this.acceptedMembersSource.asObservable();
  lobbyQueueMembersSource = new BehaviorSubject<number[]>([]);
  lobbyQueueMembers$ = this.lobbyQueueMembersSource.asObservable();
  lobbyPartyMembersSource = new BehaviorSubject<number[]>([]);
  lobbyPartyMembers$ = this.lobbyPartyMembersSource.asObservable();
  kickedPartyMembersSource = new BehaviorSubject<number>(0);
  kickedPartyMembers$ = this.kickedPartyMembersSource.asObservable();
  kickedQueueMembersSource = new BehaviorSubject<number>(0);
  kickedQueueMembers$ = this.kickedQueueMembersSource.asObservable();

  constructor() {}
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
        this.lobbyPartyMembersSource.next(ids[1]);
        this.acceptedMembersSource.next(ids[1]);
        console.log("User with id: " + ids[1] + " was accepted!");
      });
      // Everyone in lobby will get this
      this.hubConnection.on("MemberDeclined", ids => {
        this.kickedQueueMembersSource.next(ids[1]);
        console.log("User with id: " + ids[1] + " was declined!");
      });
      // Everyone in lobby will get this
      this.hubConnection.on("MemberBanned", id => {
        console.log("User with id: " + id + " was banned!");
      });
      // Everyone in lobby will get this
      this.hubConnection.on("MemberKicked", ids => {
        this.kickedPartyMembersSource.next(ids[1])
        console.log("User with id: " + ids[1] + " was kicked!");
      });

      // Everyone in lobby will get this
      this.hubConnection.on("JoinedLobbyQueue", id => {
        this.lobbyQueueMembersSource.next(id);
        console.log("User with id: " + id + " joined lobby queue!");
      });

      // Only caller will get this
      this.hubConnection.on("QueueMembers", ids => {
        ids.forEach((id: number[]) => {
          this.lobbyQueueMembersSource.next(id);
        })
        console.log("Getting users in queue");
      });
      // Only caller will get this
      this.hubConnection.on("LobbyMembers", ids => {
        ids.forEach((id: number[]) => {
          this.lobbyPartyMembersSource.next(id);
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



  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }

  addUserToObservable(id: any) {
    this.lobbyQueueMembersSource.next(id);
  }
}
