import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
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

  constructor(private authService: AuthService, private lobbyHubService: LobbyHubService, private route: ActivatedRoute, private translateService: TranslateService) {
    this.authService.loggedIn$.subscribe(
      (status) => {
        this.loggedIn = status;
        if(status){
          if(this.lobbyHubService.connectionStatus == null)
          {
            this.lobbyHubService.createHubConnection(this.authService.getUserId());
          }
        }
      }
    );
    if(localStorage.getItem('selectedLang') == undefined){
      localStorage.setItem('selectedLang', 'en');
    }
    translateService.addLangs(['en', 'nb']);
    translateService.use(localStorage.getItem('selectedLang')!)

  }

  ngOnInit() {

  }

}