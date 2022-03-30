import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Game } from 'src/app/_models/game.model';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { GamesService } from 'src/app/_services/games.service';
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

  constructor(private route: ActivatedRoute, private gamesService: GamesService, private userService: UserService) { 
    this.lobby = this.route.snapshot.data['lobby'];
    console.log(this.lobby);
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
  }

  ngOnInit(): void {
    
  }
}