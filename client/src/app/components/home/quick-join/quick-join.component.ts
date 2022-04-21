import { Component, OnInit } from '@angular/core';
import { Lobby } from 'src/app/_models/lobby.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';

@Component({
  selector: 'app-quick-join',
  templateUrl: './quick-join.component.html',
  styleUrls: ['./quick-join.component.css']
})
export class QuickJoinComponent implements OnInit {
  gameRooms: Lobby[] = [
    {id: 1, maxUsers: 5, title: 'test', lobbyDescription: '', gameId: 5, steamId: 1, gameType: 'Ranked', users: [1,2], adminUsername: 'Test', gameName: 'Some Game', adminUid: 1, lobbyRequirement: {gender: ''}, adminProfilePic: 'https://www.pngfind.com/pngs/m/610-6104451_image-placeholder-png-user-profile-placeholder-image-png.png'},
    {id: 1, maxUsers: 5, title: 'test', lobbyDescription: '', gameId: 3, steamId: 1, gameType: 'Ranked', users: [1,2], adminUsername: 'Test', gameName: 'Some Game', adminUid: 1, lobbyRequirement: {gender: ''}, adminProfilePic: 'https://www.pngfind.com/pngs/m/610-6104451_image-placeholder-png-user-profile-placeholder-image-png.png'},
    {id: 1, maxUsers: 5, title: 'test', lobbyDescription: '', gameId: 2, steamId: 1, gameType: 'Ranked', users: [1,2], adminUsername: 'Test', gameName: 'Some Game', adminUid: 1, lobbyRequirement: {gender: ''}, adminProfilePic: 'https://www.pngfind.com/pngs/m/610-6104451_image-placeholder-png-user-profile-placeholder-image-png.png'},
  ]
  constructor(private lobbyHubService: LobbyHubService) { }

  ngOnInit(): void {
    
  }

  requestToJoin(id: number){
    this.lobbyHubService.goInQueue(id);
  }
}