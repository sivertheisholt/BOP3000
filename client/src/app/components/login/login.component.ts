import { Component, OnInit } from '@angular/core';
import { faEnvelope, faLock } from '@fortawesome/free-solid-svg-icons';

import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  faEnvelope = faEnvelope; faLock = faLock;
  model: any = {};

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  login() {
    this.accountService.login(this.model).subscribe(res => {
      console.log(res);
    }, err => {
      console.log(err);
    })
  }
  
  logout() {
    this.accountService.logout();
  }

}
