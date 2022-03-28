import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { Game } from 'src/app/_models/game.model';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { LobbyService } from 'src/app/_services/lobby.service';

@Component({
  selector: 'app-account-lobby-history',
  templateUrl: './account-lobby-history.component.html',
  styleUrls: ['./account-lobby-history.component.css']
})
export class AccountLobbyHistoryComponent implements OnInit {
  @Input('finishedLobbies') finishedLobbies? : any[];
  @Input('games') games? : Game[];

  constructor(){ 

  }

  ngOnInit(): void {

  }

}
