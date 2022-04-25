import { Component, OnInit } from '@angular/core';
import { Lobby } from 'src/app/_models/lobby.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { LobbyService } from 'src/app/_services/lobby.service';

@Component({
  selector: 'app-quick-join',
  templateUrl: './quick-join.component.html',
  styleUrls: ['./quick-join.component.css']
})
export class QuickJoinComponent implements OnInit {
  recommendedLobbies: Lobby[] = [];
  lobbyId: number = 0;
  inQueue: boolean = false;
  queueStatus: string = '';

  constructor(private lobbyHubService: LobbyHubService, private lobbyService: LobbyService) { }

  ngOnInit(): void {
    this.lobbyHubService.inQueue.subscribe(
      (res) => {
        this.lobbyId = res.lobbyId;
        this.inQueue = res.inQueue;
        this.queueStatus = res.inQueueStatus;
      }
    )

    this.lobbyService.fetchRecommendedLobbies().subscribe(
      (res) => {
        this.recommendedLobbies = res;
      }
    )
    
  }

  requestToJoin(id: number){
    this.lobbyHubService.goInQueue(id);
  }

  cancelJoin(id: number){
    this.lobbyHubService.leaveQueue(id);
  }
}