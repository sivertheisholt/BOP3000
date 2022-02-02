import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faUser, faUserCircle, faSignOutAlt, faHome, faCogs, faQuestionCircle, faBullseye } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})

export class SidebarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle; faHome = faHome; faCogs = faCogs; faQuestionCircle = faQuestionCircle; faBullseye = faBullseye;
  sidebarVisible! : boolean;

  constructor(private accountService: AccountService, private router: Router) {
  }

  ngOnInit(): void {
  }

  logOut(){
    this.accountService.logout();
    this.router.navigate(['/']);
  }
}