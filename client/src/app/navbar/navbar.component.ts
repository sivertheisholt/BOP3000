import { Component, OnInit } from '@angular/core';
import { faUser, faSignOutAlt, faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle;
  constructor(private accountService : AccountService) { }

  ngOnInit(): void {
  }

  logOut(){
    this.accountService.logout();
  }
}
