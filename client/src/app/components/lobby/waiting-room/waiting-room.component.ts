import { Component, Input, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { faMinus, faPlus } from '@fortawesome/free-solid-svg-icons';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.css']
})
export class WaitingRoomComponent implements OnInit {
  message!: string;
  faMinus = faMinus; faPlus = faPlus;
  usersInQueue? : Member[] = [];
  @Input('lobby') lobby! : Lobby;

  constructor(private lobbyHubService: LobbyHubService, private userService: UserService) { 
    
  }

  ngOnInit(): void {
    this.lobbyHubService.lobbyQueueMembers$.subscribe(
      member => {
        if(member.length == 0) return;
        this.userService.getSpecificUser(+member).subscribe(
          (response) => {
            this.usersInQueue?.push(response);
          }
        )
      },
      error => console.log(error)
    )
  }

  denyUserToJoin(index: number){
    if(index > -1){
      this.usersInQueue?.splice(index, 1);
    }
  }

  acceptUserToJoin(uid: number){
    console.log("accept");
    this.lobbyHubService.acceptMember(this.lobby.id, uid);
  }
}


