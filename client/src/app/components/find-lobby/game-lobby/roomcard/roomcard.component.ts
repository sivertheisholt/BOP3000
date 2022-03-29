import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  filteredLobbies: Lobby[] = [];
  inQueue? : boolean;
  currentId?: number;
  dropdownStatus: boolean = false;

  constructor(private route: ActivatedRoute, private lobbyHubService: LobbyHubService, private lobbyService: LobbyService) { }

  ngOnInit(): void {
    this.lobbies = this.route.snapshot.data['lobbies'];
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

  cancelJoin(id: number){
    this.lobbyHubService.leaveQueue(id);
    this.inQueue = false;
  }

  toggleDropdown(){
    this.dropdownStatus = !this.dropdownStatus;
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
    for(let lobby of this.route.snapshot.data['lobbies']){
      if(lobby.gameType == type){
        this.lobbies.push(lobby);
      }
    }
  }

  resetFilter(){
    this.lobbies = this.route.snapshot.data['lobbies'];
  }
}
