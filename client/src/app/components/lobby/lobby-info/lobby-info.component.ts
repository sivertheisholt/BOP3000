import { Component, Input, OnInit } from '@angular/core';
import { Game } from 'src/app/_models/game.model';
import { Lobby } from 'src/app/_models/lobby.model';

@Component({
  selector: 'app-lobby-info',
  templateUrl: './lobby-info.component.html',
  styleUrls: ['./lobby-info.component.css']
})
export class LobbyInfoComponent implements OnInit {
  @Input('lobby') lobby? : Lobby;
  @Input('game') game? : Game;

  constructor() { }

  ngOnInit(): void {
  }

}
