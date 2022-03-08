import { Injectable } from '@angular/core';
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

  lobbyQueueMembersSource = new BehaviorSubject<Number[]>([]);
  lobbyQueueMembers$ = this.lobbyQueueMembersSource.asObservable();
  lobbyPartyMembersSource = new BehaviorSubject<Number[]>([]);
  lobbyPartyMembers$ = this.lobbyPartyMembersSource.asObservable();

  constructor() {}

  createHubConnection(token: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'lobby', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build()

      this.hubConnection
        .start()
        .catch(error => console.log(error));

      // Member that is accepted will get this
      this.hubConnection.on('Accepted', () => {
        console.log("You have been accepted!");
      })
      // Member that is declined will get this
      this.hubConnection.on('Declined', () => {
        console.log("You have been declined!");
      })
      // Member that is declined will get this
      this.hubConnection.on('Banned', () => {
        console.log("You have been declined!");
      })

      // Everyone in lobby will get this
      this.hubConnection.on("MemberAccepted", id => {
        this.lobbyPartyMembersSource.next(id);
        console.log("User with id: " + id + " was accepted!");
      })
      // Everyone in lobby will get this
      this.hubConnection.on("MemberDeclined", id => {
        this.lobbyPartyMembersSource.next(id);
        console.log("User with id: " + id + " was declined!");
      })
      // Everyone in lobby will get this
      this.hubConnection.on("MemberBanned", id => {
        this.lobbyPartyMembersSource.next(id);
        console.log("User with id: " + id + " was banned!");
      })

      // Everyone in lobby will get this
      this.hubConnection.on("JoinedLobbyQueue", id => {
        this.lobbyQueueMembersSource.next(id);
        console.log("User with id: " + id + " joined lobby queue!");
      })

      // Only caller will get this
      this.hubConnection.on("QueueMembers", ids => {
        ids.forEach((id: Number[]) => {
          this.lobbyQueueMembersSource.next(id);
        })
        console.log("Getting users in queue");
      })
      // Only caller will get this
      this.hubConnection.on("LobbyMembers", ids => {
        ids.forEach((id: Number[]) => {
          this.lobbyPartyMembersSource.next(id);
        })
        console.log("Getting users in lobby");
      })
  }

  goInQueue(lobbyId: number){
    this.hubConnection.invoke("JoinQueue", lobbyId);
  }
  getQueueMembers(lobbyId: number){
    this.hubConnection.invoke("GetQueueMembers", lobbyId);
  }
  getLobbyMembers(lobbyId: number){
    this.hubConnection.invoke("GetLobbyMembers", lobbyId);
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }

  addUserToObservable(id: any) {
    this.lobbyQueueMembersSource.next(id);
  }
}
