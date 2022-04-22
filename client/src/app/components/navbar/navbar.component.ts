import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { faUser, faSignOutAlt, faUserCircle, faBell, faHome, faCogs, faBullseye, faQuestionCircle, faUsers, faGlobe } from '@fortawesome/free-solid-svg-icons';
import { TranslateService } from '@ngx-translate/core';
import { NotifierService } from 'angular-notifier';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, map, takeUntil } from 'rxjs/operators';
import { Member } from 'src/app/_models/member.model';
import { NotificationModel } from 'src/app/_models/notification.model';
import { UserSearch } from 'src/app/_models/user-search.model';
import { AuthService } from 'src/app/_services/auth.service';
import { LobbyHubService } from 'src/app/_services/lobby-hub.service';
import { LobbyService } from 'src/app/_services/lobby.service';
import { NotificationService } from 'src/app/_services/notification.service';
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
  searchResults: UserSearch[] = [];
  isNotiVisible = false;
  isNavVisible = false;
  notifications: NotificationModel[] = [];
  totalNotifications: number = 0;
  inLobby: boolean = false;
  inLobbyId: number = 0;
  inLobbyStatus: string = '';
  user?: Member;
  private destroyStreamSource = new Subject<void>()

  constructor(private authService: AuthService,
     private router: Router,
      private userService: UserService,
       private notifierService: NotifierService,
        private notificationService: NotificationService,
         private translateService: TranslateService,
          private lobbyService: LobbyService,
           private lobbyHubService: LobbyHubService){}

  ngOnInit(): void {
    this.userService.getUserData().subscribe(
      (res) => {
        this.user = res;
      }
    )
    this.notificationService.notificationObservable
    .pipe(takeUntil(this.destroyStreamSource))
    .subscribe((notification: NotificationModel) => {
      this.notifications.push(notification);
      this.notifierService.notify(notification.type, notification.message);
      this.totalNotifications++;
    })

    

    this.lobbyService.getQueueStatus().subscribe(
      (res) => {
        console.log(res);
        if(res.lobbyId != 0 && !res.inQueue){
          this.inLobby = true;
          this.inLobbyId = res.lobbyId;
          this.inLobbyStatus = 'accepted';
        }
      }
    )

    this.lobbyHubService.inQueue$.subscribe(
      (res) => {
        console.log(res);
        this.inLobbyId = res.lobbyId;
        this.inLobby = res.inQueue;
        this.inLobbyStatus = res.inQueueStatus;
      }
    )

    this.router.events.subscribe(
      (val) => {
        this.isNavVisible = false;
      }
    )

    fromEvent(this.searchInput?.nativeElement, 'keyup').pipe(
      map((event: any) => {
        if(event.target.value.length > 2){
          this.searchInput?.nativeElement.classList.add('loading');
          return event.target.value;
        }
        this.searchInput?.nativeElement.classList.remove('loading');
        this.searchResults = [];
      })
      ,filter(res => res != undefined)
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

  changeLang(lang: string){
    this.translateService.use(lang);
    localStorage.setItem('selectedLang', lang);
  }
}
