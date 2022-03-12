import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-joined-users',
  templateUrl: './joined-users.component.html',
  styleUrls: ['./joined-users.component.css']
})
export class JoinedUsersComponent implements OnInit{
  faPlus = faPlus;
  usersInParty : Member[] = [];
  usersInPartyTotal: number = 0;
  @Input('lobby') lobby! : Lobby;
  @Output() totalUsersInPartyEvent = new EventEmitter<number>();

  constructor(private lobbyHubService: LobbyHubService, private userService: UserService) {
    this.lobbyHubService.lobbyPartyMembers$.subscribe(
      member => {
        if(member.length == 0) return;
        this.userService.getSpecificUser(+member).subscribe(
          (response) => {
            this.usersInParty.push(response);
            this.totalUsersInPartyEvent.emit(this.usersInParty.length);
          }
        )
      },
      error => console.log(error)
    )

    this.lobbyHubService.kickedPartyMembers$.subscribe(
      response => {
        this.usersInParty.forEach(element => {
          if(response == element.id){
            let index = this.usersInParty.indexOf(element);
            this.usersInParty.splice(index, 1);
          }
        });
      }
    )
  }

  ngOnInit(): void {
    this.lobbyHubService.getLobbyMembers(this.lobby.id);
  }

  kickFromParty(uid: number){
    this.lobbyHubService.kickMemberFromParty(this.lobby.id, uid);
  }
}
