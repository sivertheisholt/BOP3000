import { Component, ElementRef, OnInit, Renderer2, ViewChild, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { Observable } from 'rxjs';
import { UserProfile } from 'src/app/_models/user-profile.model';

import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { UserService } from 'src/app/_services/user.service';

import { MemberCardComponent } from './member-card/member-card.component';

@Component({
  selector: 'app-waiting-room',
  templateUrl: './waiting-room.component.html',
  styleUrls: ['./waiting-room.component.css']
})
export class WaitingRoomComponent implements OnInit {
  message!: string;
  @ViewChild('queueLobby', { read: ViewContainerRef })
  container!: ViewContainerRef;

  constructor(private lobbyHubService: LobbyHubService, private componentFactoryResolver: ComponentFactoryResolver, private userService: UserService) { 
    
  }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.lobbyHubService.lobbyQueueMembers$.subscribe(
      member => {
        //console.log(member)
        if(member.length == 0) return;
        this.memberJoinedQueue(MemberCardComponent);
      },
      error => console.log(error)
    )
  }

  memberJoinedQueue(componentClass: any): void {
    
    // Create component dynamically inside the ng-template
    const componentFactory = this.componentFactoryResolver.resolveComponentFactory(componentClass);
    const component = this.container.createComponent(componentFactory);
  }

  updateQueueList(componentClass: any, users: Observable<string[]>): void {
    users.forEach(user => {
      //console.log(user)
      // Create component dynamically inside the ng-template
      const componentFactory = this.componentFactoryResolver.resolveComponentFactory(componentClass);
      const component = this.container.createComponent(componentFactory);
    })
  }
}


