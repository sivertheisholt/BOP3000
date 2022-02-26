import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class LobbyHubService {

  hubUrl = environment.hubUrl;
  private hubConnection!: HubConnection;

  constructor() {}

  createHubConnection(token: string, lobbyId: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'lobby?lobbyId=' + lobbyId, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build()

      this.hubConnection
        .start()
        .catch(error => console.log(error));

      this.hubConnection.on('JoinedLobbyQueue', id => {

        console.log("User with id: " + id + " has connected to lobby");
      })

      this.hubConnection.on('LeftLobby', id => {
        console.log("User with id: " + id + " has left");
      })
      this.hubConnection.on("LeftQueue", id => {
        console.log("User with id: " + id + " has left Queue");
      })
      this.hubConnection.on("JoinedLobbyQueue", id => {
        console.log("User with id: " + id + " has joined lobby queue");
      })
      this.hubConnection.on("MemberAccepted", id => {
        console.log("User with id: " + id + " was accepted");
      })
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }

  
}
