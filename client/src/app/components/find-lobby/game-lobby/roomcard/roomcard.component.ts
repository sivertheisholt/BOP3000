import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { Game } from 'src/app/_models/game.model';
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
  loadedLobbies: Lobby[] = [];
  inQueue? : boolean;
  currentId?: number;
  dropdownStatus: boolean = false;
  @Input('game') game?: Game;
  pageNumber: number = 1;
  pageSize: number = 10;
  inQueueStatus?: string;

  constructor(private lobbyHubService: LobbyHubService, private lobbyService: LobbyService) { }

  ngOnInit(): void {
    this.getLobbies();
    this.lobbyService.getQueueStatus().subscribe(
      (response) => {
        this.inQueue = response.inQueue;
        this.currentId = response.lobbyId;
      }
    );
    this.lobbyHubService.inQueue.subscribe(
      (res) => {
        this.inQueueStatus = res;
      }
    )
  }

  requestToJoin(id: number){
    this.lobbyHubService.goInQueue(id);
    this.inQueue = true;
    this.currentId = id;
  }

  cancelJoin(id: number){
    this.lobbyHubService.leaveQueue(id);
    this.inQueue = false;
  }

  toggleDropdown(){
    this.dropdownStatus = !this.dropdownStatus;
  }

  onScroll(){
    this.pageNumber++;
    this.lobbyService.fetchAllLobbiesWithGameIdTest(this.game?.id!, this.pageNumber, this.pageSize).subscribe(
      (res) => {
        res.forEach(lobby => {
          this.lobbies.push(lobby);
        })
        
      }
    )
  }

  filterLobby(id: number){
    switch (id) {
      case 1:
        this.filterLobbies('Competetive');
        break;
      case 2:
        this.filterLobbies('Ranked');
        break;
      case 3:
        this.filterLobbies('Fun');
        break;
      case 4:
        this.filterLobbies('Tryhard');
        break;
      case 5:
        this.filterLobbies('Chill');
        break;
      case 6:
        this.filterLobbies('Casual');
        break;
      case 7:
        this.resetFilter();
        break;
      default:
        this.resetFilter();
        break;
    }
  }

  filterLobbies(type: string){
    this.lobbies = [];
    for(let lobby of this.loadedLobbies){
      if(lobby.gameType == type){
        this.lobbies.push(lobby);
      }
    }
  }

  resetFilter(){
    this.lobbies = this.loadedLobbies;
  }

  getLobbies(){
    this.lobbyService.fetchAllLobbiesWithGameIdTest(this.game?.id!, this.pageNumber, this.pageSize).subscribe(
      (res) => {
        this.lobbies = res;
        this.loadedLobbies = res;
      }
    )
  }
}
