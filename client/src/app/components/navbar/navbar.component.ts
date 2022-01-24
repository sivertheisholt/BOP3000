import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { faUser, faSignOutAlt, faUserCircle } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from 'src/app/_services/navigation.service';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle;
  @ViewChild('navBurger') navBurger!: ElementRef;
  @ViewChild('navMenu') navMenu!: ElementRef;

  constructor(private accountService : AccountService, public navService: NavigationService) { }

  ngOnInit(): void {
  }
  
  toggleNavbar(){
    this.navService.toggleNav();
  }

  logOut(){
    console.log("Hello")
    this.accountService.logout();
  }
}
