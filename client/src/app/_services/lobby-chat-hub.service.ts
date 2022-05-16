import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ChatMessage } from '../_models/chatmessage.model';

@Injectable({
  providedIn: 'root'
})
export class LobbyChatHubService {

  hubUrl = environment.hubUrl;
  private hubConnection!: HubConnection;
  connectionStatus!: Promise<boolean>;

  chatMembersSource$ = new BehaviorSubject<number[]>([]);
  chatAllMessagesSource$ = new BehaviorSubject<ChatMessage[]>([]);

  constructor() {}

  getChatMembersObserver(): Observable<number[]>{
    return this.chatMembersSource$.asObservable();
  }

  getChatAllMessagesObserver(): Observable<ChatMessage[]>{
    return this.chatAllMessagesSource$.asObservable();
  }

  createHubConnection(token: string, lobbyId: string) {
    this.connectionStatus = new Promise(resolve => {
      this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'lobby-chat?lobbyId=' + lobbyId, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect()
      .build()

      this.hubConnection
        .start()
        .then(() => resolve(true))
        .catch(error => console.log(error));


    })
      // Everyone will get this except the caller
      this.hubConnection.on('JoinedChat', id => {
        this.chatMembersSource$.next(id);
      })

      this.hubConnection.on('GetMessages', messages => {
        this.chatAllMessagesSource$.next(messages);
      })

      this.hubConnection.on('NewMessage', message => {
        this.chatAllMessagesSource$.next(message);
      })
  }

  async sendMessage(lobbyId: number, message: string){
    await this.connectionStatus;
    this.hubConnection.invoke("SendMessage", lobbyId, message);
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }
}
