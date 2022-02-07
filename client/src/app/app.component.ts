import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AuthService } from './_services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  title = 'BOP3000';
  users: any;
  loggedIn : boolean;

  constructor(private authService: AuthService) {
    this.loggedIn = authService.isLoggedIn;
  }

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user')!);
    this.authService.setCurrentUser(user);
  }
}