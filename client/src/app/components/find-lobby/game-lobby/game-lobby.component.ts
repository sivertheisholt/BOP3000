import { Component, OnInit } from '@angular/core';
import { GamesService } from 'src/app/_services/games.service';
import { LobbyService } from 'src/app/_services/lobby.service';

@Component({
  selector: 'app-game-lobby',
  templateUrl: './game-lobby.component.html',
  styleUrls: ['./game-lobby.component.css']
})
export class GameLobbyComponent implements OnInit {

  constructor(private lobbyService: LobbyService) { }

  ngOnInit(): void {
    this.lobbyService.fetchAllLobbies().subscribe(
      (res) => {
        console.log(res);
      }
    );
  }

}
