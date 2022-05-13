import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faUser, faUserCircle, faSignOutAlt, faHome, faCogs, faQuestionCircle, faBullseye, faUsers, faGlobe } from '@fortawesome/free-solid-svg-icons';
import { TranslateService } from '@ngx-translate/core';
import { Member } from 'src/app/_models/member.model';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})

export class SidebarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle; faHome = faHome; faCogs = faCogs; faQuestionCircle = faQuestionCircle; faBullseye = faBullseye; faUsers = faUsers; faGlobe = faGlobe;
  sidebarVisible! : boolean;
  user?: Member;

  constructor(private authService: AuthService, private router: Router, private userService: UserService, private translateService: TranslateService) {
    this.userService.getUserData().subscribe(
      (res) => {
        this.user = res;
      }
    )
  }

  ngOnInit(): void {
  }

  logOut(){
    this.authService.logout();
    this.router.navigate(['/']);
  }

  changeLang(lang: string){
    this.translateService.use(lang);
    localStorage.setItem('selectedLang', lang);
  }
}