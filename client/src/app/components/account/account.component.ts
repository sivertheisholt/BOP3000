import { Component, OnInit } from '@angular/core';

import { faThumbsDown, faThumbsUp } from '@fortawesome/free-regular-svg-icons';
import { UserProfile } from 'src/app/_models/user-profile.model';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  faThumbsDown = faThumbsDown; faThumbsUp = faThumbsUp;
  user?: UserProfile;

  constructor(private userService: UserService) {
    this.userService.getUserData().subscribe(
      (response) => {
        this.user = response;
        console.log(this.user);
      }
    );
  }

  ngOnInit(): void {
/*     this.userService.getUserData().subscribe(
      (response) => {
        console.log(response);
      }
    ) */
  }

}
