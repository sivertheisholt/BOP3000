import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
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
  loginUserForm! : FormGroup;

  constructor(public accountService: AccountService, public router: Router) { }

  ngOnInit(): void {
    this.loginUserForm = new FormGroup({
      username: new FormControl(null),
      password: new FormControl(null)
    });
  }

  login() {
    this.accountService.login(this.loginUserForm.value).subscribe(res => {
      console.log(res);
      this.router.navigate(['/home']);
    }, err => {
      console.log(err);
    })
  }
  
  logout() {
    this.accountService.logout();
  }

}
