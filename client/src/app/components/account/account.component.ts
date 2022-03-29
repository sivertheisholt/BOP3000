import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Game } from 'src/app/_models/game.model';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { GamesService } from 'src/app/_services/games.service';
import { LobbyService } from 'src/app/_services/lobby.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  user?: Member;
  currentUser?: Member;
  finishedLobbies: Lobby[] = [];
  games: Game[] = [];

  constructor(private route: ActivatedRoute, private userService: UserService, private lobbyService: LobbyService, private gamesService: GamesService) {
    this.userService.getSpecificUser(this.route.snapshot.params.id).subscribe(
      (response) => {
        this.user = response;
        response.memberProfile?.memberData?.finishedLobbies?.forEach(lobbyId => {
          this.lobbyService.fetchLobbyWithId(lobbyId).subscribe(
            (response) => {
              this.finishedLobbies.push(response);
              this.gamesService.fetchGame(response.gameId).subscribe(
                (response) => {
                  this.games.push(response)
                }
              )
            }
          )
        })
      }
    )

    this.userService.getUserData().subscribe(
      (response) => {
        this.currentUser = response;
      }
    )
  }

  ngOnInit(): void {
    
  }
}
