import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { faThumbsDown, faThumbsUp } from '@fortawesome/free-solid-svg-icons';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { LobbyService } from 'src/app/_services/lobby.service';
import { NotificationService } from 'src/app/_services/notification.service';
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

  constructor(private userService: UserService, private route: ActivatedRoute, private lobbyService: LobbyService, private notificationService: NotificationService) { }

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
    this.lobby?.votes!.forEach((vote) => {
      console.log(vote);
    })

  }

  upvoteUser(id: number){
    console.log("User with id " + this.lobby?.id + " upvoted user with id " + id);
    this.lobbyService.upvoteUser(this.lobby?.id!, id).subscribe(
      (res) => {
        this.notificationService.setNewNotification({type: 'success', message: 'Upvoted'});
      }, error => {
        this.notificationService.setNewNotification({type: 'error', message: 'Already Voted'});
      }
    );
  }

  downvoteUser(id: number){
    console.log("User with id " + this.lobby?.id + " downvoted user with id " + id);
    this.lobbyService.downvoteUser(this.lobby?.id!, id).subscribe(
      (res) => {
        this.notificationService.setNewNotification({type: 'success', message: 'Downvoted'});
      }, error => {
        this.notificationService.setNewNotification({type: 'error', message: 'Already Voted'});
      }
    );
  }

}
