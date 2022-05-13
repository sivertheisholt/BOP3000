import { DOCUMENT } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { faCheckCircle } from '@fortawesome/free-solid-svg-icons';
import { DiscordConnection } from 'src/app/_models/discord-connection.model';
import { SteamConnection } from 'src/app/_models/steam-connection.model';
import { ConnectionService } from 'src/app/_services/connection.service';
import { UserSettingsService } from 'src/app/_services/user-settings.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-connections',
  templateUrl: './connections.component.html',
  styleUrls: ['./connections.component.css']
})
export class ConnectionsComponent implements OnInit {
  faCheckCircle = faCheckCircle;
  connectToSteamForm!: FormGroup;
  discordData?: DiscordConnection;
  steamData?: SteamConnection;
  steamHidden?: boolean;
  discordHidden?: boolean;

  constructor(@Inject(DOCUMENT) private document: Document, private connectionService: ConnectionService, private userService: UserService, private userSettingsService: UserSettingsService ) {}
  
  ngOnInit(): void {
    this.userService.getUserData().subscribe(
      (user) => {
        this.userService.getDiscordConnectionStatus(user.id!).subscribe(
          (discordResponse) => {
            this.discordData = discordResponse;
            console.log(discordResponse);
            this.discordHidden = discordResponse.hidden;
          }
        )
        this.userService.getSteamConnectionStatus(user.id!).subscribe(
          (steamResponse) => {
            this.steamData = steamResponse;
            this.steamHidden = steamResponse.hidden;
          }
        )
      }
    )
  }
  onSteamUnlink(){
    this.connectionService.disconnectFromSteam().subscribe(
      (res) => {
        
      }
    )
  }

  onDiscordUnlink(){
    this.connectionService.disconnectFromDiscord().subscribe(
      (res) => {

      }
    )
  }

  onDiscordHide(e: any){
    if(e.target.checked){
      this.hideDiscord(true);
      this.discordHidden = true;
      return;
    } else {
      this.hideDiscord(false);
      this.discordHidden = false;
      return;
    }
  }

  onSteamHide(e: any){
    if(e.target.checked){
      this.hideSteam(true);
      this.steamHidden = true;
      return;
    } else {
      this.hideSteam(false);
      this.steamHidden = false;
      return;
    }
  }

  hideSteam(status: boolean){
    this.userSettingsService.hideSteam(status).subscribe(
      (res) => {
        return;
      }
    )
  }

  hideDiscord(status: boolean){
    this.userSettingsService.hideDiscord(status).subscribe(
      (res) => {
        return;
      }
    )
  }
}

