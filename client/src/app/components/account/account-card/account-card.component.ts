import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { faArrowAltCircleUp, faThumbsDown, faThumbsUp } from '@fortawesome/free-solid-svg-icons';
import { Member } from 'src/app/_models/member.model';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-account-card',
  templateUrl: './account-card.component.html',
  styleUrls: ['./account-card.component.css']
})
export class AccountCardComponent implements OnInit {
  faThumbsDown = faThumbsDown; faThumbsUp = faThumbsUp; faArrowAltCircleUp = faArrowAltCircleUp;
  @Input('user') user? : Member;
  @Input('currentUser') currentUser?: Member;
  isFollowing?: boolean;

  constructor(private userService: UserService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.userService.checkFollowing(this.route.snapshot.params.id).subscribe(
      (res) => {
        this.isFollowing = res;
      }
    )
  }

  onFollowUser(){
    this.userService.followUser(this.user?.id!).subscribe(
      (res) => {
        this.isFollowing = !this.isFollowing;
      }
    )
  }

  onUnFollowUser(){
    this.userService.unfollowUser(this.user?.id!).subscribe(
      (res) => {
        this.isFollowing = !this.isFollowing;
      }
    )
  }

  changeProfilePicture(){

  }

}
