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

      // Everyone will get this except the caller
      this.hubConnection.on('JoinedLobbyQueue', id => {
        this.lobbyQueueMembersSource.next(id);
        console.log("User with id: " + id + " has connected to lobby queue");
      })

      // Everyone will get this
      this.hubConnection.on("LeftLobbyQueue", id => {
        console.log("User with id: " + id + " has left Queue");
      })

      // Everyone will get this
      this.hubConnection.on('LeftLobby', id => {
        console.log("User with id: " + id + " has left");
      })

      // Everyone will get this
      this.hubConnection.on("MemberAccepted", id => {
        this.lobbyPartyMembersSource.next(id);
        console.log("User with id: " + id + " was accepted");
      })

      //Only caller will get this
      this.hubConnection.on("GetQueueMembers", ids => {
        ids.forEach((id: Number[]) => {
          this.lobbyQueueMembersSource.next(id);
        })
        console.log("Getting users");
      })

      this.hubConnection.on("test",() => {
        
        console.log("DETTE ER EN TEST");
      })
  }

  goInQueue(lobbyId: number){
    this.hubConnection.invoke("OnQueuePending", lobbyId);
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }

  addUserToObservable(id: any) {
    this.lobbyQueueMembersSource.next(id);
  }
}
