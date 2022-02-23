import { Component, OnInit } from '@angular/core';

import { faThumbsDown, faThumbsUp } from '@fortawesome/free-regular-svg-icons';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  faThumbsDown = faThumbsDown; faThumbsUp = faThumbsUp;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.getUserData().subscribe(
      (response) => {
        console.log(response);
      }
    )
  }

}
