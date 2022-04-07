import { DOCUMENT } from '@angular/common';
import { Component, ElementRef, Inject, OnInit, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { faCheckCircle } from '@fortawesome/free-solid-svg-icons';
import { DiscordConnection } from 'src/app/_models/discord-connection.model';
import { SteamConnection } from 'src/app/_models/steam-connection.model';
import { ConnectionService } from 'src/app/_services/connection.service';
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

  constructor(@Inject(DOCUMENT) private document: Document, private connectionService: ConnectionService, private userService: UserService ) {}
  
  ngOnInit(): void {
    this.userService.getUserData().subscribe(
      (user) => {
        this.userService.getDiscordConnectionStatus(user.id!).subscribe(
          (discordResponse) => {
            this.discordData = discordResponse;
            
          }
        )
        this.userService.getSteamConnectionStatus(user.id!).subscribe(
          (steamResponse) => {
            this.steamData = steamResponse;
          }
        )
      }
    )
  }

  onSteamLink(){
    this.connectionService.connectToSteam().subscribe(
      (res: any) => {
        this.document.location.href = res.url!;
      }
    )
  }

  onSteamUnlink(){

  }

  onDiscordLink(){
    this.connectionService.connectToDiscord().subscribe(
      (res: any) => {
        this.document.location.href = res.url!;
      }
    )
  }

  onDiscordUnlink(){

  }

  onDiscordHide(e: any){
    if(e.target.checked){
      console.log('Hiding discord from profile');
      return;
    } else {
      console.log('Making discord visible');
      return;
    }
  }

  onSteamHide(e: any){
    if(e.target.checked){
      console.log('Hiding steam from profile');
      return;
    } else {
      console.log('Making steam visible');
      return;
    }
  }
}