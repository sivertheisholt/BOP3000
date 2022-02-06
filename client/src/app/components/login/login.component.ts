import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { faEnvelope, faLock } from '@fortawesome/free-solid-svg-icons';

import { AuthService } from '../../_services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  faEnvelope = faEnvelope; faLock = faLock;
  model: any = {};
  loginUserForm! : FormGroup;

  constructor(public authService: AuthService, public router: Router) { }

  ngOnInit(): void {
    this.loginUserForm = new FormGroup({
      username: new FormControl(null, Validators.required),
      password: new FormControl(null, Validators.required)
    });
  }

  login() {
    this.authService.login(this.loginUserForm.value).subscribe(res => {
      console.log(res);
      this.router.navigate(['/home']);
    }, err => {
      console.log(err);
    })
  }
  
  logout() {
    this.authService.logout();
  }

}
