import { Component, ElementRef, OnInit, Renderer2, ViewChild } from '@angular/core';
import { faMinus, faPlus, faCheck } from '@fortawesome/free-solid-svg-icons';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.css']
})
export class WaitingRoomComponent implements OnInit {
  faMinus = faMinus; faPlus = faPlus;
  @ViewChild('queueLobby') queueLobby!: ElementRef;

  constructor(private lobbyHubService: LobbyHubService, private renderer:Renderer2) { 
    
  }

  ngOnInit(): void {
  }

  acceptMember(): void {
    let memberDiv = this.renderer.createElement('div');
    this.renderer.setProperty(memberDiv, 'id', 'member-card');
    this.renderer.appendChild(this.queueLobby, memberDiv);
    console.log("Hi accept");
  }

  kickMemberQueue(): void {
    console.log("Hi remove");
  }

}
