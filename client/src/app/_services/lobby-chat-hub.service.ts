import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LobbyChatHubService {

  hubUrl = environment.hubUrl;
  private hubConnection!: HubConnection;

  chatMembersSource = new BehaviorSubject<Number[]>([]);
  chatMembersSource$ = this.chatMembersSource.asObservable();

  constructor() {}

  createHubConnection(token: string, lobbyId: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'lobby-chat?lobbyId=' + lobbyId, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build()

      this.hubConnection
        .start()
        .catch(error => console.log(error));

      // Everyone will get this except the caller
      this.hubConnection.on('JoinedChat', id => {
        console.log("User with id: " + id + " has connected to chat");
      })
      this.hubConnection.on('GetMessages', messages => {
        console.log("Getting messages");
      })
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }
}
