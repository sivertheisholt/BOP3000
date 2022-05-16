import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { LobbyHubService } from './lobby-hub.service';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = environment.apiUrl;

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  loggedInSource = new BehaviorSubject<boolean>(false);
  loggedIn$ = this.loggedInSource.asObservable();

  constructor(private http: HttpClient, private lobbyHub: LobbyHubService) {
  }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'accounts/login', model).pipe(
      map((res: User) => {
        const user = res;
        if(user) {
          this.initCurrentUser(res);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post<User>(this.baseUrl + 'accounts/register', model).pipe(
      map((res: User) => {
        const user = res;
        if(user) {
          this.initCurrentUser(res);
        }
      })
    )
  }

  updateUserPassword(model : any){
    return this.http.patch(this.baseUrl + 'accounts/change_password', model);
  }

  postEmailForgottenPassword(email: string){
    return this.http.post(this.baseUrl + 'accounts/forgotten_password', {
      email
    });
  }

  updateUserPasswordFromEmail(body: any){
    return this.http.patch(this.baseUrl + 'accounts/change_forgotten_password?token=' + this.getUserId(), body).pipe(
      map(response => {
        return response;
      })
    );
  }

  initCurrentUser(user: User) {
    localStorage.setItem('token', JSON.stringify(user.token));
    this.currentUserSource.next(user);
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(undefined);
    this.lobbyHub.stopHubConnection();
    this.loggedInSource.next(false);
  }

  get isLoggedIn(): boolean{
    let authToken = localStorage.getItem('token');
    if(authToken !== null){
      this.loggedInSource.next(true);
      return true;
    }
    this.loggedInSource.next(false);
    return false;
    //return (authToken !== null) ? true : false;
  }

  getUserId(){
    return JSON.parse(localStorage.getItem('token')!);
  }
}