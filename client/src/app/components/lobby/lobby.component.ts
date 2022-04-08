import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Game } from 'src/app/_models/game.model';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { GamesService } from 'src/app/_services/games.service';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.css']
})
export class LobbyComponent implements OnInit {
  lobby: Lobby;
  game!: Game;
  currentUser? : Member;
  voiceUrl: string = '';

  constructor(private route: ActivatedRoute, private gamesService: GamesService, private userService: UserService, private lobbyHubService: LobbyHubService) { 
    this.lobby = this.route.snapshot.data['lobby'];
    this.gamesService.fetchGame(this.lobby.gameId).subscribe(
      (response) => {
        this.game = response;
      }
    );

    this.userService.getUserData().subscribe(
      (response) => {
        this.currentUser = response;
      }
    );

    this.lobbyHubService.lobbyStart$.subscribe(
      (url) => {
        this.voiceUrl = url;
      }
    )

  }

  ngOnInit(): void {
    
  }
}