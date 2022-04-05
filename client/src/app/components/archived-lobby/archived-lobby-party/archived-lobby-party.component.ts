import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  currentUser!: Member;
  canVote: boolean = false;
  alreadyVoted: boolean = false;

  constructor(private userService: UserService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.currentUser = this.route.snapshot.data['user'];
    this.lobby?.users.forEach((user) => {
      if(user == this.currentUser.id){
        this.canVote = true;
      }
      this.userService.getSpecificUser(user).subscribe(
        (res) => {
          this.users?.push(res);
        }
      )
    });

  }

  upvoteUser(id: number){
    this.alreadyVoted = true;
    console.log("User with id " + this.currentUser.id + " upvoted user with id " + id);
  }

  downvoteUser(id: number){
    this.alreadyVoted = true;
    console.log("User with id " + this.currentUser.id + " downvoted user with id " + id);
  }

}
