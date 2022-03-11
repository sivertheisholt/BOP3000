import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterEvent } from '@angular/router';
import { Lobby } from 'src/app/_models/lobby.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { LobbyService } from 'src/app/_services/lobby.service';

@Component({
  selector: 'app-roomcard',
  templateUrl: './roomcard.component.html',
  styleUrls: ['./roomcard.component.css']
})
export class RoomcardComponent implements OnInit {
  lobbies: Lobby[] = [];
  inQueue? : boolean;
  currentId?: number;


  constructor(private route: ActivatedRoute, private lobbyHubService: LobbyHubService, private lobbyService: LobbyService) { }

  ngOnInit(): void {
    this.lobbies = this.route.snapshot.data['posts'];
    this.lobbyService.getLobbyStatus().subscribe(
      (response) => {
        this.inQueue = response.inQueue;
        this.currentId = response.lobbyId;
      }
    );
  }

  requestToJoin(id: number){
    this.lobbyHubService.goInQueue(id);
    this.inQueue = true;
  }

  cancelJoin(){
    this.inQueue = false;
  }
}
