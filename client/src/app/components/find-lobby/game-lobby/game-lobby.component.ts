import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Game } from 'src/app/_models/game.model';
import { GamesService } from 'src/app/_services/games.service';

@Component({
  selector: 'app-game-lobby',
  templateUrl: './game-lobby.component.html',
  styleUrls: ['./game-lobby.component.css']
})
export class GameLobbyComponent implements OnInit {
  gameInfo? : Game;
  myStyles= {
    width: '100%',
    height: '100vh',
    background: 'url(https://cdn.akamai.steamstatic.com/steam/apps/1438160/page_bg_generated_v6b.jpg?t=1602738467)'
  }
  constructor(private gamesService: GamesService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.gamesService.fetchGame(this.route.snapshot.params.id).subscribe(
      (res) => {
        this.gameInfo = res;
      }
    )
  }

}
