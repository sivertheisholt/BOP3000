import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Game } from 'src/app/_models/game.model';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { AuthService } from 'src/app/_services/auth.service';
import { GamesService } from 'src/app/_services/games.service';
import { LobbyChatHubService } from 'src/app/_services/lobby-chat-hub.service';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { LobbyService } from 'src/app/_services/lobby.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-lobby',
  templateUrl: './lobby.component.html',
  styleUrls: ['./lobby.component.css']
})
export class LobbyComponent implements OnInit {
  lobby: Lobby;
  game!: Game;
  hostUser? : Member;
  queueMembers : Number[] = [];

  constructor(private route: ActivatedRoute, private gamesService: GamesService, private userService: UserService, private lobbyChatHub: LobbyChatHubService) { 
    this.lobby = route.snapshot.data['lobby'];
    this.gamesService.fetchGame(this.lobby.gameId).subscribe(
      (response) => {
        this.game = response;
      }
    );
    this.userService.getSpecificUser(2).subscribe(
      (response) => {
        this.hostUser = response;
        //console.log(this.hostUser);
      }
    )
  }

  ngOnInit(): void {
    
  }
}
