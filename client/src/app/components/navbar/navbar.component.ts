import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { faUser, faSignOutAlt, faUserCircle, faBell, faHome, faCogs, faBullseye, faQuestionCircle } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from 'src/app/_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle; faBell = faBell; faHome = faHome; faCogs = faCogs; faQuestionCircle = faQuestionCircle; faBullseye = faBullseye;
  @ViewChild('navBurger') navBurger!: ElementRef;
  @ViewChild('navMenu') navMenu!: ElementRef;
  isNotiVisible = false;
  isNavVisible = false;
  notifications = [
    {id: 1, text: 'test 1'},
    {id: 2, text: 'test 2'}
  ];
  totalNotifications = this.notifications.length;

  constructor(private accountService: AccountService, private router: Router){ 
  }

  ngOnInit(): void {
  }
  
  toggleNavbar(){
    this.isNavVisible = !this.isNavVisible;
  }

  toggleNotifications(){
    this.totalNotifications = 0;
    this.isNotiVisible = !this.isNotiVisible;
  }

  logOut(){
    this.accountService.logout();
    this.router.navigate(['/']);
  }
}
