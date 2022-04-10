import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { environment } from 'src/environments/environment';
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
  navEnabled?: boolean;

  constructor(private authService: AuthService, private lobbyHubService: LobbyHubService, private route: ActivatedRoute) {
    this.loggedIn = authService.isLoggedIn;
    if(this.loggedIn){
      this.lobbyHubService.createHubConnection(this.authService.getUserId());
    }
    this.authService.navEnabled$.subscribe(
      (status) => {
        this.navEnabled = status;
      }
    )
  }

  ngOnInit() {
    this.setCurrentUser();
  }


  setCurrentUser() {
    if(this.loggedIn) {
      
    }
  }
}