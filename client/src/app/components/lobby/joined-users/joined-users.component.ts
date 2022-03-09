import { Component, OnInit } from '@angular/core';
import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { Member } from 'src/app/_models/member.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-joined-users',
  templateUrl: './joined-users.component.html',
  styleUrls: ['./joined-users.component.css']
})
export class JoinedUsersComponent implements OnInit {
  faPlus = faPlus;
  usersInParty : Member[] = [];
  constructor(private lobbyHubService: LobbyHubService, private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getUserData().subscribe(
      (response) => {
        this.usersInParty.push(response);
      }
    )
    this.lobbyHubService.lobbyPartyMembers$.subscribe(
      member => {
        console.log(member);
        if(member.length == 0) return;
        this.userService.getSpecificUser(+member).subscribe(
          (response) => {
            console.log(response);
            this.usersInParty.push(response);
          }
        )
      },
      error => console.log(error)
    )
  }

  kickFromParty(index: number){
    if(index > -1){
      this.usersInParty.splice(index, 1);
    }
  }
}
