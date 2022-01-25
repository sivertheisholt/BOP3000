import { Component, OnInit } from '@angular/core';
import { faUser, faUserCircle, faSignOutAlt, faHome, faCogs, faQuestionCircle, faBullseye } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from 'src/app/_services/account.service';
import { NavigationService } from 'src/app/_services/navigation.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})

export class SidebarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle; faHome = faHome; faCogs = faCogs; faQuestionCircle = faQuestionCircle; faBullseye = faBullseye;
  sidebarVisible! : boolean;

  constructor(public navService: NavigationService, public accountService: AccountService) { 
  }

  ngOnInit(): void {
    this.navService.sidebarVisibleChange.subscribe(
      (value) => {
        this.sidebarVisible = value;
      }
    );
  }

  logOut(){
    console.log("Hello")
    this.accountService.logout();
  }
}