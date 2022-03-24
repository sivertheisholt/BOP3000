import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member.model';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  user?: Member;
  currentUser?: Member;

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
  }

  ngOnInit(): void {
    
  }
}
