import { AfterContentInit, AfterViewInit, Component, DoCheck, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
export class AccountComponent implements OnInit, DoCheck {

  user?: Member;
  currentUser?: Member;
  finishedLobbies: Lobby[] = [];
  bgUrl = '';

  constructor(private route: ActivatedRoute, private userService: UserService, private lobbyService: LobbyService) {

  }

  ngOnInit(): void {
    this.route.params.subscribe(
      (val: any)  => {
        this.finishedLobbies = [];
        this.userService.getSpecificUser(val.id).subscribe(
          (response) => {
            this.user = response;
            this.bgUrl = this.user.memberProfile?.memberCustomization.backgroundUrl!;
            this.user.memberProfile?.memberData?.finishedLobbies?.forEach(lobbyId => {
              this.lobbyService.fetchLobbyWithId(lobbyId).subscribe(
                (response) => {
                  response.startDate = this.fixDate(response.startDate!);
                  this.finishedLobbies.push(response);
                }
              )
            })
          }
        )
      }
    )

    this.userService.getUserData().subscribe(
      (response) => {
        this.currentUser = response;
      }
    )
  }

  ngDoCheck(): void {
    this.finishedLobbies.sort((a, b) => new Date(b.startDate!).getTime() - new Date(a.startDate!).getTime());
  }

  fixDate(lobbyStartDate: string){
    let date = new Date(lobbyStartDate);
    let fixedDate = ('0' + date.getDate()).slice(-2) + '.' + ('0' + date.getMonth()).slice(-2) + '.' + ('0' + date.getFullYear()).slice(-2);
    return fixedDate;
  }
}
