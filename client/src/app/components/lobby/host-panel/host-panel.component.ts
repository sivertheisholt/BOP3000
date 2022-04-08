import { Component, Input, OnInit } from '@angular/core';
import { faCheckCircle, faThumbsDown } from '@fortawesome/free-solid-svg-icons';
import { Subscription, timer } from 'rxjs';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-host-panel',
  templateUrl: './host-panel.component.html',
  styleUrls: ['./host-panel.component.css']
})
export class HostPanelComponent implements OnInit {
  readyCheckModal?: boolean;
  faCheckCircle = faCheckCircle;
  endLobbyModal: boolean = false;
  subscription?: Subscription;
  start = 30;
  @Input('lobby') lobby?: Lobby;
  usersAccepted: number = 0;
  userDeclined: number = 0;
  totalParty: number = 0;
  accepted: boolean = false;
  declined: boolean = false;

  constructor(private lobbyHubService: LobbyHubService) { }

  ngOnInit(): void {
    this.lobbyHubService.lobbyReadyCheck$.subscribe(
      (res) => {
        if(res){
          this.readyCheckModal = res;
          this.readyCheckTimer();
        }
      }
    )

    this.lobbyHubService.lobbyPartyMembers$.subscribe(
      member => {
        if(+member == 0) return;
        this.totalParty++;
      },
      error => console.log(error)
    )

    this.lobbyHubService.kickedPartyMembers$.subscribe(
      response => {
        this.totalParty--;
      }
    )

    this.lobbyHubService.acceptedReadyCheckMembers$.subscribe(
      (res) => {
        if(res.length == 0) return;
        this.usersAccepted++;
      }
    )

    this.lobbyHubService.declinedReadyCheckMembers$.subscribe(
      (res) => {
        this.userDeclined = res;
        this.subscription?.unsubscribe();
        this.start = 30;
        setTimeout(() => {
          this.readyCheckModal = false;
        }, 3000)
      }
    )

    this.lobbyHubService.lobbyStart$.subscribe(
      (res) => {
        if(res != ''){
          this.subscription?.unsubscribe();
          this.start = 30;
          this.readyCheckModal = false;
        }
      }
    )
  }
  

  startReadyCheck(){
    this.lobbyHubService.startReadyCheck(this.lobby?.id!);
    this.accepted = true;
  }

  readyCheckTimer(){
    const source = timer(1000, 1000);
    this.subscription = source.subscribe(
      val => {
        this.start = this.start - 1;
        if(this.start === 0){
          this.subscription?.unsubscribe();
          this.start = 30;
          this.readyCheckModal = false;
        }
      }
    )
  }

  confirmEndLobby(){
    this.endLobbyModal = !this.endLobbyModal;
  }
  
  closeEndLobbyModal(){
    this.endLobbyModal = false;
  }
}