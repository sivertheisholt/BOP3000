import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { faMinus, faPlus, faCheck } from '@fortawesome/free-solid-svg-icons';
import { UserProfile } from 'src/app/_models/user-profile.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})

export class MemberCardComponent implements OnInit {
  faMinus = faMinus; faPlus = faPlus;
  message: string = "Hello!"
  user?: UserProfile;

  @Output() acceptMemberEvent = new EventEmitter<string>();
  
  constructor(private userService: UserService, private lobbyHubService: LobbyHubService) {
    this.lobbyHubService.lobbyQueueMembers$.subscribe(
      (id) => {
        this.userService.getSpecificUser(+id).subscribe(
          (response) => {
            console.log(response);
            this.user = response;
          }
        )
      }
        
      
    )

  }

  ngOnInit(): void {
  }
  acceptMember() {
    this.acceptMemberEvent.emit()
  } 
  
}
