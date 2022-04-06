import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { faMinus, faPlus } from '@fortawesome/free-solid-svg-icons';
import { Subscription } from 'rxjs';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.css']
})
export class WaitingRoomComponent implements OnInit, OnDestroy {
  message!: string;
  faMinus = faMinus; faPlus = faPlus;
  usersInQueue : Member[] = [];
  @Input('lobby') lobby! : Lobby;
  @Input('currentUser') currentUser?: Member;
  lobbyQueueMembersSubscription?: Subscription;

  constructor(private lobbyHubService: LobbyHubService, private userService: UserService) { 

  }

  ngOnInit(): void {
    this.lobbyHubService.getQueueMembers(this.lobby.id);

    this.lobbyQueueMembersSubscription = this.lobbyHubService.lobbyQueueMembers$.subscribe(
      member => {
        if(+member == 0) return;
        this.userService.getSpecificUser(+member).subscribe(
          (response) => {
            this.usersInQueue?.push(response);
          }
        )
      },
      error => console.log(error)
    )

    this.lobbyHubService.kickedQueueMembers$.subscribe(
      response => {
        this.usersInQueue?.forEach(user => {
          if(user.id == response){
            let index = this.usersInQueue?.indexOf(user);
            this.usersInQueue?.splice(index!, 1);
          }
        });
      }
    )

    this.lobbyHubService.acceptedMembers$.subscribe(
      response => {
        this.usersInQueue?.forEach(user => {
          if(user.id == response){
            let index = this.usersInQueue?.indexOf(user);
            this.usersInQueue?.splice(index!, 1);
          }
        });
      }
    )
  }

  ngOnDestroy(): void {
    this.lobbyQueueMembersSubscription?.unsubscribe();
  }

  denyUserToJoin(uid: number){
    this.lobbyHubService.declineMemberFromQueue(this.lobby.id, uid);
  }

  acceptUserToJoin(uid: number){
    this.lobbyHubService.acceptMemberFromQueue(this.lobby.id, uid);
  }
}


