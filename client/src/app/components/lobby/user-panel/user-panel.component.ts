import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faCheckCircle } from '@fortawesome/free-solid-svg-icons';
import { Subscription, timer } from 'rxjs';
import { Lobby } from 'src/app/_models/lobby.model';
import { Member } from 'src/app/_models/member.model';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['./user-panel.component.css']
})
export class UserPanelComponent implements OnInit {
  leaveLobbyStatus: boolean = false;
  readyCheckModal?: boolean;
  faCheckCircle = faCheckCircle;
  subscription?: Subscription;
  start = 30;
  @Input('currentUser') currentUser?: Member;
  @Input('lobby') lobby!: Lobby;
  usersAccepted: number = 0;
  userDeclined: number = 0;
  totalParty: number = 0;
  accepted: boolean = false;
  declined: boolean = false;

  constructor(private lobbyHubService: LobbyHubService, private router: Router) { }

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

  leaveLobbyModal(){
    this.leaveLobbyStatus = true;
  }

  leaveLobby(){
    this.leaveLobbyStatus = false;
    this.lobbyHubService.leaveParty(this.lobby.id);
    this.router.navigate(['/home']);
  }

  goBack(){
    this.leaveLobbyStatus = false;
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

  acceptReadyCheck(){
    this.lobbyHubService.acceptReadyCheck(this.lobby?.id!);
    this.accepted = true;
  }

  declineReadyCheck(){
    this.lobbyHubService.declineReadyCheck(this.lobby?.id!);
    this.declined = true;
  }

}
