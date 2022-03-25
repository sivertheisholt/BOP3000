import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['./user-panel.component.css']
})
export class UserPanelComponent implements OnInit {
  leaveLobbyStatus: boolean = false;
  @Input('currentUser') currentUser?: Member;
  @Input('lobby') lobby!: Lobby;

  constructor(private lobbyHubService: LobbyHubService, private router: Router) { }

  ngOnInit(): void {
  }

  leaveLobbyModal(){
    this.leaveLobbyStatus = true;
  }

  leaveLobby(){
    this.leaveLobbyStatus = false;
    this.lobbyHubService.leaveParty(this.lobby.id);
    this.router.navigate(['/home']);
  }

  goBack(){
    this.leaveLobbyStatus = false;
  }

}
