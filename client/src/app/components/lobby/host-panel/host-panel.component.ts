import { Component, OnInit } from '@angular/core';
import { Subscription, timer } from 'rxjs';

@Component({
  selector: 'app-host-panel',
  templateUrl: './host-panel.component.html',
  styleUrls: ['./host-panel.component.css']
})
export class HostPanelComponent implements OnInit {
  readyCheckModal: boolean = false;
  endLobbyModal: boolean = false;
  subscription?: Subscription;
  start = 30;

  constructor() { }

  ngOnInit(): void {
  }

  readyCheck(){
    this.readyCheckModal = !this.readyCheckModal;
    const source = timer(1000, 1000);
    this.subscription = source.subscribe(
      val => {
        this.start = this.start - 1;
        if(this.start === 0){
          this.subscription?.unsubscribe();
        }
      }
    )
  }

  acceptReadyCheck(){
    this.readyCheckModal = false;
    this.subscription?.unsubscribe();
    this.start = 30;
  }

  declineReadyCheck(){
    this.subscription?.unsubscribe();
    this.start = 30;
  }

  confirmEndLobby(){
    this.endLobbyModal = !this.endLobbyModal;
  }
  
  closeEndLobbyModal(){
    this.endLobbyModal = false;
  }
}