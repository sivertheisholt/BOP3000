import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { faUser, faSignOutAlt, faUserCircle, faBell } from '@fortawesome/free-solid-svg-icons';
import { AccountService } from 'src/app/_services/account.service';
import { NavigationService } from 'src/app/_services/navigation.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle; faBell = faBell;
  @ViewChild('navBurger') navBurger!: ElementRef;
  @ViewChild('navMenu') navMenu!: ElementRef;
  notifications = [
    {id: 1, text: 'test'},
    {id: 2, text: 'test 2'}
  ];

  constructor(public navService: NavigationService){ }

  ngOnInit(): void {
  }
  
  toggleNavbar(){
    this.navService.toggleNav();
  }
}
