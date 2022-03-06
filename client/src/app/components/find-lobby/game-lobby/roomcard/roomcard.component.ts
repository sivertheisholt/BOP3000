import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterEvent } from '@angular/router';
import { Lobby } from 'src/app/_models/lobby.model';
import { AuthService } from 'src/app/_services/auth.service';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { LobbyService } from 'src/app/_services/lobby.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-roomcard',
  templateUrl: './roomcard.component.html',
  styleUrls: ['./roomcard.component.css']
})
export class RoomcardComponent implements OnInit {
  lobbies: Lobby[] = [];
  waitingToJoin = false;
  goTo = false;


  constructor(private lobbyService: LobbyService, private route: ActivatedRoute, private lobbyHubService: LobbyHubService, private authService: AuthService) { }

  ngOnInit(): void {
    this.lobbies = this.route.snapshot.data['posts'];
  }

  requestToJoin(id: number){
    //this.lobbyHubService.createHubConnection(this.authService.getUserId(), id.toString());
    this.lobbyHubService.goInQueue(id);
    this.waitingToJoin = true;
  }

  cancelJoin(){
    this.waitingToJoin = false;
  }

}
