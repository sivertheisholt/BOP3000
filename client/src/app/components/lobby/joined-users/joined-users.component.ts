import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { faCrown } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-joined-users',
  templateUrl: './joined-users.component.html',
  styleUrls: ['./joined-users.component.css']
})
export class JoinedUsersComponent implements OnInit, OnDestroy{
  faCrown = faCrown;
  usersInParty : Member[] = [];
  totalParty: number = 0;
  @Input('lobby') lobby! : Lobby;
  @Input('currentUser') currentUser?: Member;
  lobbyPartyMembersSubscription?: Subscription;

  constructor(private lobbyHubService: LobbyHubService, private userService: UserService) {
  }

  ngOnInit(): void {
    this.lobbyHubService.getLobbyMembers(this.lobby.id);

    this.lobbyPartyMembersSubscription = this.lobbyHubService.lobbyPartyMembers$.subscribe(
      member => {
        console.log(member);
        if(+member == 0) return;
        this.userService.getSpecificUser(+member).subscribe(
          (response) => {
            this.totalParty++;
            this.usersInParty.push(response);
          }
        )
      },
      error => console.log(error)
    )

    this.lobbyHubService.kickedPartyMembers$.subscribe(
      response => {
        this.usersInParty.forEach(element => {
          if(response == element.id){
            this.totalParty--;
            let index = this.usersInParty.indexOf(element);
            this.usersInParty.splice(index, 1);
          }
        });
      }
    )
  }

  ngOnDestroy(): void {
    this.lobbyPartyMembersSubscription?.unsubscribe();
  }

  kickFromParty(uid: number){
    this.lobbyHubService.kickMemberFromParty(this.lobby.id, uid);
  }
}
