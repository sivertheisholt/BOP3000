import { Component, Input, OnInit } from '@angular/core';
import { faCrown } from '@fortawesome/free-solid-svg-icons';
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
  faCrown = faCrown;
  usersInParty : Member[] = [];
  count: number = 0;
  @Input('lobby') lobby! : Lobby;

  constructor(private lobbyHubService: LobbyHubService, private userService: UserService) {
    this.lobbyHubService.getLobbyPartyMembersObserver().subscribe(
      member => {
        if(member.length == 0) return;
        this.userService.getSpecificUser(+member).subscribe(
          (response) => {
            this.usersInParty.push(response);
          }
        )
      },
      error => console.log(error)
    )

    this.lobbyHubService.getLobbyKickedPartyMembersObserver().subscribe(
      response => {
        this.usersInParty.forEach(element => {
          if(response == element.id){
            let index = this.usersInParty.indexOf(element);
            this.usersInParty.splice(index, 1);
          }
        });
      }
    )

    this.lobbyHubService.getLobbyPartyMembersObserver().subscribe(
      member => {
        if(member.length == 0) return;
        this.count++;
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
