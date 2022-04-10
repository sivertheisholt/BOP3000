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
  loggedIn?: boolean;

  constructor(private authService: AuthService, private lobbyHubService: LobbyHubService, private route: ActivatedRoute) {
    this.authService.loggedIn$.subscribe(
      (status) => {
        this.loggedIn = status;
        if(status){
          this.lobbyHubService.createHubConnection(this.authService.getUserId());
        }
      }
    )
  }

  ngOnInit() {

  }

}