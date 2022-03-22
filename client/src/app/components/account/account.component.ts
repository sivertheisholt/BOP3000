import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { faThumbsDown, faThumbsUp, faArrowAltCircleUp } from '@fortawesome/free-regular-svg-icons';
import { Member } from 'src/app/_models/member.model';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  faThumbsDown = faThumbsDown; faThumbsUp = faThumbsUp; faArrowAltCircleUp = faArrowAltCircleUp;
  user?: Member;
  currentUser?: Member;
  isFollowing?: boolean;

  constructor(private route: ActivatedRoute, private userService: UserService) {
    this.userService.getSpecificUser(this.route.snapshot.params.id).subscribe(
      (response) => {
        this.user = response;
      }
    )

    this.userService.getUserData().subscribe(
      (response) => {
        this.currentUser = response;
      }
    )

    this.userService.checkFollowing(this.route.snapshot.params.id).subscribe(
      (res) => {
        this.isFollowing = res;
      }
    )
  }

  ngOnInit(): void {
    
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
