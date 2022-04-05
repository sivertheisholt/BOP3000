import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Game } from 'src/app/_models/game.model';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { GamesService } from 'src/app/_services/games.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-archived-lobby',
  templateUrl: './archived-lobby.component.html',
  styleUrls: ['./archived-lobby.component.css']
})
export class ArchivedLobbyComponent implements OnInit {
  lobby: Lobby;
  game!: Game;

  constructor(private route: ActivatedRoute, private gamesService: GamesService, private userService: UserService) { 
    this.lobby = this.route.snapshot.data['lobby'];
    
    this.gamesService.fetchGame(this.lobby.gameId).subscribe(
      (response) => {
        this.game = response;
      }
    );
  }

  ngOnInit(): void {
    
  }

}
