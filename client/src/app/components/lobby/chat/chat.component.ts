import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ChatMessage } from 'src/app/_models/chatmessage.model';
import { Lobby } from 'src/app/_models/lobby.model';
import { AuthService } from 'src/app/_services/auth.service';
import { LobbyChatHubService } from 'src/app/_services/lobby-chat-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  @Input('lobby') lobby! : Lobby;
  @ViewChild('chatMessage') chatMessage!: ElementRef;
  chatMessages : ChatMessage[] = []

  constructor(private lobbyChatHubService: LobbyChatHubService, private authService: AuthService) {}

  ngOnInit(): void {
    this.lobbyChatHubService.createHubConnection(this.authService.getUserId(), this.lobby.id.toString());

    this.lobbyChatHubService.getChatAllMessagesObserver().subscribe(
      (messages) => {
        messages.forEach((message) => {
          message.dateSent = this.convertDate(message.dateSent)
          this.chatMessages.push(message);
        });
      }
    )
  }

  ngOnDestroy(): void {
    this.lobbyChatHubService.stopHubConnection();
  }

  onSubmit(){
    if(this.chatMessage.nativeElement.value.length !== 0){
      this.lobbyChatHubService.sendMessage(this.lobby.id, this.chatMessage.nativeElement.value);
      this.chatMessage.nativeElement.value = '';
      this.chatMessage.nativeElement.focus();
    }
  }

  convertDate(date: string){
    let d = new Date(date);
    let hour = d.getHours().toString();
    let minutes = d.getMinutes().toString();
    let seconds = d.getSeconds().toString();
    if(hour.length < 2) hour = '0' + hour;
    if(minutes.length < 2) minutes = '0' + minutes;
    if(seconds.length < 2) seconds = '0' + seconds;
    return hour + ':' + minutes + ':' + seconds;
  }
}