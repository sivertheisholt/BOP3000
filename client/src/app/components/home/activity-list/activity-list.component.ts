import { Component, OnInit } from '@angular/core';
import { Lobby } from 'src/app/_models/lobby.model';

@Component({
  selector: 'app-activity-list',
  templateUrl: './activity-list.component.html',
  styleUrls: ['./activity-list.component.css']
})
export class ActivityListComponent implements OnInit {
  gameRooms: Lobby[] = [
    {id: 1, maxUsers: 5, title: 'test', lobbyDescription: '', gameId: 5, steamId: 1, gameType: 'Ranked', users: [1,2], adminUid: 1, lobbyRequirement: {gender: ''}},
    {id: 2, maxUsers: 5, title: 'test', lobbyDescription: '', gameId: 3, steamId: 1, gameType: 'Ranked', users: [1,2], adminUid: 1, lobbyRequirement: {gender: ''}},
    {id: 3, maxUsers: 5, title: 'test', lobbyDescription: '', gameId: 2, steamId: 1, gameType: 'Ranked', users: [1,2], adminUid: 1, lobbyRequirement: {gender: ''}},

  ]
  constructor() { }

  ngOnInit(): void {
  }

}
