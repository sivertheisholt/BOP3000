import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { LobbyChatHubService } from 'src/app/_services/lobby-chat-hub.service';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.css']
})
export class LobbyComponent implements OnInit {

  constructor(private route: ActivatedRoute, private lobbyHub: LobbyHubService, private authService: AuthService, private lobbyChatHub: LobbyChatHubService) { }

  ngOnInit(): void {
    this.lobbyHub.createHubConnection(this.authService.getUserId(), this.route.snapshot.params['id']);
    this.lobbyChatHub.createHubConnection(this.authService.getUserId(), this.route.snapshot.params['id']);
    console.log();
  }
}
