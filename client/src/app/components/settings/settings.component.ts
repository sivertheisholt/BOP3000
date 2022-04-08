import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ConnectionService } from 'src/app/_services/connection.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {
  viewMode = 'tab1';
  constructor(private route: ActivatedRoute, private connectionService: ConnectionService) { 
    console.log(this.route.snapshot.queryParamMap);
    if(this.route.snapshot.queryParams.success != null){
      if(this.route.snapshot.queryParams.provider == "discord") this.discordSuccess()
      if(this.route.snapshot.queryParams.provider == "steam") this.steamSuccess();
    }
  }

  ngOnInit(): void {
    
  }

  steamSuccess(): void {
    this.connectionService.onSteamSuccess().subscribe(
      (res) => {
        this.viewMode = 'tab3';
      }
    )
  }

  discordSuccess(): void {
    this.connectionService.onDiscordSuccess().subscribe(
      (res) => {
        this.viewMode = 'tab3';
      }
    )
  }
}
