import { Component, Input, OnInit } from '@angular/core';
import { faCopy } from '@fortawesome/free-solid-svg-icons'
@Component({
  selector: 'app-join-voice',
  templateUrl: './join-voice.component.html',
  styleUrls: ['./join-voice.component.css']
})
export class JoinVoiceComponent implements OnInit {
  faCopy = faCopy;
  @Input('voiceUrl') voiceUrl: string = '';

  constructor() { }

  ngOnInit(): void {
  }


  copyLink(input: any){
    input.select();
    input.setSelectionRange(0, 99999);
    navigator.clipboard.writeText(input.value);
  }

  joinVoiceChat(){
    window.open(this.voiceUrl, '_blank');
  }
}

