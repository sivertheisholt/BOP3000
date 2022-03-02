import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-manage-account',
  templateUrl: './manage-account.component.html',
  styleUrls: ['./manage-account.component.css']
})
export class ManageAccountComponent implements OnInit {
  status: boolean = false;
  constructor(private userService: UserService) { }

  ngOnInit(): void {
  }

  toggleModal(){
    this.status = !this.status;
  }

  onDeleteAccount(){
    this.userService.deleteAccount().subscribe(
      (response) => {
        console.log(response);
      }
    )
  }

}
