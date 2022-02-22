import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { min } from 'rxjs/operators';
import { ChatMessage } from 'src/app/_models/chatmessage.model';

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

  constructor() { }

  ngOnInit(): void {
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
