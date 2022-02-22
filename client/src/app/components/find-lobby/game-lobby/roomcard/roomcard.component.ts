import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterEvent } from '@angular/router';
import { Lobby } from 'src/app/_models/lobby.model';
import { LobbyService } from 'src/app/_services/lobby.service';

@Component({
  selector: 'app-roomcard',
  templateUrl: './roomcard.component.html',
  styleUrls: ['./roomcard.component.css']
})
export class RoomcardComponent implements OnInit {

  constructor(private lobbyService: LobbyService, private route: ActivatedRoute) { }
  lobbies: Lobby[] = [];

  ngOnInit(): void {
    this.lobbies = this.route.snapshot.data['posts'];
/*     this.lobbyService.fetchAllLobbies()
      .subscribe(
        (response) => {
          console.log(response);
          this.lobbies = response;
          console.log(this.lobbies);
          
        }
      ); */

  }

}
