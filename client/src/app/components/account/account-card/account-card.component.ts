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
  imageHeightWidth: boolean = false;
  imageSize: boolean = false;
  errorStatus: boolean = false;
  isBlocked: boolean = false;

  constructor(private userService: UserService, private route: ActivatedRoute) { 
    
  }
  
  ngOnInit(): void {
    this.userService.checkFollowing(this.route.snapshot.params.id).subscribe(
      (res) => {
        this.isFollowing = res;
      }
    )
    this.userService.checkIfBlocking(this.route.snapshot.params.id).subscribe(
      (res) => {
        this.isBlocked = res;
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

  blockUser(){
    this.userService.blockUser(this.user?.id!).subscribe(
      (res) => {
        this.isBlocked = true;
      }, error => {
        this.isBlocked = false;
      }
    )
  }

  unblockUser(){
    this.userService.unblockUser(this.user?.id!).subscribe(
      (res) => {
        this.isBlocked = false;
      }, error => {
        this.isBlocked = true;
      }
    )
  }

  onUploadProfilePicture(event: any){
    const formData: FormData = new FormData();
    if(event.target.files[0].size > 500 * 500){
      this.imageHeightWidth = true;
      return;
    }
    if(event.target.files[0].size > 3000000){
      this.imageSize = true;
      return;
    }
    this.imageHeightWidth = false;
    this.imageSize = false;
    this.errorStatus = false;
    formData.append('File', event.target.files[0], event.target.files[0].name);
    this.userService.postProfileImage(formData).subscribe(
      (res) => {
        this.user!.memberProfile!.memberPhoto = res;
      },(error) => {
        this.errorStatus = true;
        return;
      }
    );
  }

}
