import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { faThumbsDown, faThumbsUp } from '@fortawesome/free-regular-svg-icons';
import { Member } from 'src/app/_models/member.model';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  faThumbsDown = faThumbsDown; faThumbsUp = faThumbsUp;
  user?: Member;

  constructor(private route: ActivatedRoute, private userService: UserService) {
    this.userService.getSpecificUser(this.route.snapshot.params.id).subscribe(
      (response) => {
        this.user = response;
      }
    )
  }

  ngOnInit(): void {
    
  }

}
