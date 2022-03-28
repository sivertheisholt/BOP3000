import { Component, ElementRef, OnChanges, OnDestroy, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faUser, faSignOutAlt, faUserCircle, faBell, faHome, faCogs, faBullseye, faQuestionCircle } from '@fortawesome/free-solid-svg-icons';
import { fromEvent } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
import { UserSearch } from 'src/app/_models/user-search.model';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle; faBell = faBell; faHome = faHome; faCogs = faCogs; faQuestionCircle = faQuestionCircle; faBullseye = faBullseye;
  @ViewChild('navBurger') navBurger!: ElementRef;
  @ViewChild('navMenu') navMenu!: ElementRef;
  @ViewChild('searchInput', {static: true}) searchInput? : ElementRef;
  searchResults: any;
  isNotiVisible = false;
  isNavVisible = false;
  notifications = [
    {id: 1, text: 'test 1'},
    {id: 2, text: 'test 2'}
  ];
  totalNotifications = this.notifications.length;

  constructor(private authService: AuthService, private router: Router, private userService: UserService){
  }

  ngOnInit(): void {
    this.router.events.subscribe(
      (val) => {
        this.isNavVisible = false;
      }
    )

    fromEvent(this.searchInput?.nativeElement, 'keyup').pipe(
      map((event: any) => {
        if(event.target.value == ''){
          this.searchResults = [];
          return event.target.value;
        }
        return event.target.value;
      })
      ,filter(res => res.length > 2)
      ,debounceTime(1000)
      ,distinctUntilChanged()
    ).subscribe((input: string) => {
      this.userService.searchUser(input).subscribe((response) => {
        this.searchResults = response;
      }, (err) => {
        console.log(err);
      })
    })
  }

  
  toggleNavbar(){
    this.isNavVisible = !this.isNavVisible;
  }

  toggleNotifications(){
    this.totalNotifications = 0;
    this.isNotiVisible = !this.isNotiVisible;
  }

  logOut(){
    this.authService.logout();
    this.router.navigate(['/']);
  }

  clearSearch(){
    this.searchResults = [];
  }
}
