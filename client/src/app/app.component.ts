import { Component, OnInit } from '@angular/core';
import { User } from './_models/user';
import { AuthService } from './_services/auth.service';
import { LobbyHubService } from './_services/lobby-hub.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent implements OnInit {
  title = 'BOP3000';
  users: any;
  loggedIn : boolean;

  constructor(private authService: AuthService, private lobbyHubService: LobbyHubService) {
    this.loggedIn = authService.isLoggedIn;
    lobbyHubService.createHubConnection(this.authService.getUserId());
  }

  ngOnInit() {
    this.setCurrentUser();
  }

  setCurrentUser() {
    if(this.loggedIn) {
      
    }
  }
}