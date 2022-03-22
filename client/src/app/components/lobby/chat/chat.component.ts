import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ChatMessage } from 'src/app/_models/chatmessage.model';
import { AuthService } from 'src/app/_services/auth.service';
import { LobbyChatHubService } from 'src/app/_services/lobby-chat-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {
  sendMessageForm! : FormGroup;
  chatMessages : ChatMessage[] = [
    { username: 'test', timestamp: new Date(), message: ' test' }
  ]

  constructor(private lobbyChatHubService: LobbyChatHubService, private userService: UserService, private authService: AuthService) { }

  ngOnInit(): void {
    this.lobbyChatHubService.createHubConnection(this.authService.getUserId(), '1');
    this.sendMessageForm = new FormGroup({
      message: new FormControl(null, [Validators.required]),
      timestamp: new FormControl(this.setTimestamp()),
      username: new FormControl('testbruker')
    })
  }

  onSubmit(){
    this.chatMessages.push(this.sendMessageForm.value);
    console.log(this.chatMessages);
  }

  setTimestamp(){
    const time = new Date();
    const hours = time.getHours();
    const minutes = time.getMinutes();
    return hours + ':' + minutes;
  }

}
