import { Component, Input, OnInit } from '@angular/core';
import { faThumbsDown, faThumbsUp } from '@fortawesome/free-solid-svg-icons';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-archived-lobby-party',
  templateUrl: './archived-lobby-party.component.html',
  styleUrls: ['./archived-lobby-party.component.css']
})
export class ArchivedLobbyPartyComponent implements OnInit {
  @Input('lobby') lobby?: Lobby;
  faThumbsUp = faThumbsUp; faThumbsDown = faThumbsDown;
  users?: Member[] = [];

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.lobby?.users.forEach((user) => {
      this.userService.getSpecificUser(user).subscribe(
        (res) => {
          this.users?.push(res);
        }
      )
    })
  }

  upvoteUser(){

  }

  downvoteUser(){
    
  }

}
