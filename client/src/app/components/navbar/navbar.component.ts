import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { faUser, faSignOutAlt, faUserCircle, faBell, faHome, faCogs, faBullseye, faQuestionCircle, faUsers, faGlobe } from '@fortawesome/free-solid-svg-icons';
import { NotifierService } from 'angular-notifier';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map, takeUntil } from 'rxjs/operators';
import { AuthService } from 'src/app/_services/auth.service';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { NotificationService } from 'src/app/_services/notification.service';
import { SpinnerService } from 'src/app/_services/spinner.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit, OnDestroy {
  faUser = faUser; faSignOutAlt = faSignOutAlt; faUserCircle = faUserCircle; faBell = faBell; faHome = faHome; faCogs = faCogs; faQuestionCircle = faQuestionCircle; faBullseye = faBullseye; faUsers = faUsers; faGlobe = faGlobe;
  @ViewChild('navBurger') navBurger!: ElementRef;
  @ViewChild('navMenu') navMenu!: ElementRef;
  @ViewChild('searchInput', {static: true}) searchInput? : ElementRef;
  searchResults: any;
  isNotiVisible = false;
  isNavVisible = false;
  notifications: string[] = [];
  totalNotifications: number = 0;
  private destroyStreamSource = new Subject<void>()

  constructor(private authService: AuthService, private router: Router, private userService: UserService, private lobbyHubService: LobbyHubService, private notifierService: NotifierService, private notificationService: NotificationService){
  }

  ngOnInit(): void {
    this.notificationService.notificationObservable
    .pipe(takeUntil(this.destroyStreamSource))
    .subscribe((message: string) => {
      this.notifications.push(message);
      this.notifierService.notify('info', message);
      this.totalNotifications = this.notifications.length;
    })

/*     this.lobbyHubService.notifyUser$.subscribe(
      (res) => {
        

      }
    ) */


    this.router.events.subscribe(
      (val) => {
        this.isNavVisible = false;
      }
    )

    fromEvent(this.searchInput?.nativeElement, 'keyup').pipe(
      map((event: any) => {
        if(event.target.value == ''){
          this.searchInput?.nativeElement.classList.remove('loading');
          this.searchResults = [];
          return event.target.value;
        }
        this.searchInput?.nativeElement.classList.add('loading');
        return event.target.value;
      })
      ,filter(res => res.length > 2)
      ,debounceTime(1000)
      ,distinctUntilChanged()
    ).subscribe((input: string) => {
      this.userService.searchUser(input).subscribe((response) => {
        this.searchInput?.nativeElement.classList.remove('loading');
        this.searchResults = response;
      }, (err) => {
        this.searchInput?.nativeElement.classList.remove('loading');
        console.log(err);
      })
    })
  }

  ngOnDestroy(): void {
    this.destroyStreamSource.next();
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
