import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faUser, faUserCircle, faSignOutAlt, faHome, faCogs, faQuestionCircle, faBullseye } from '@fortawesome/free-solid-svg-icons';
import { AuthService } from 'src/app/_services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})

export class SidebarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle; faHome = faHome; faCogs = faCogs; faQuestionCircle = faQuestionCircle; faBullseye = faBullseye;
  sidebarVisible! : boolean;

  constructor(private authService: AuthService, private router: Router) {
  }

  ngOnInit(): void {
  }

  logOut(){
    this.authService.logout();
    this.router.navigate(['/']);
  }
}