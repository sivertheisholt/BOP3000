import { Component, Input, OnInit } from '@angular/core';
import { Game } from 'src/app/_models/game.model';
import { Lobby } from 'src/app/_models/lobby.model';

@Component({
  selector: 'app-archived-lobby-info',
  templateUrl: './archived-lobby-info.component.html',
  styleUrls: ['./archived-lobby-info.component.css']
})
export class ArchivedLobbyInfoComponent implements OnInit {
  @Input('game') game?: Game;
  @Input('lobby') lobby?: Lobby;

  constructor() { }

  ngOnInit(): void {
  }

}
