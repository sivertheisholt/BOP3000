import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { catchError, map, retry } from 'rxjs/operators';
import { User } from '../_models/user';
import { LobbyHubService } from './lobby-hub.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl = 'https://localhost:5001/api/';

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

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
    console.log(JSON.stringify(model));
    return this.http.patch(this.baseUrl + 'accounts/change_password', JSON.stringify(model), {responseType: 'text'}).pipe(
      map((response) => {
        console.log(response);
      })
    ).subscribe((res) => {
      console.log(res);
    })
  }

  postEmailForgottenPassword(email: string){
    return this.http.post(this.baseUrl + 'accounts/forgotten_password', {
      email
    });
  }

  updateUserPasswordFromEmail(body: any){
    return this.http.patch(this.baseUrl + 'accounts/change_forgotten_password?token=' + this.getUserId(), body).pipe(
      map(response => {
        console.log(response);
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
  }

  get isLoggedIn(): boolean{
    let authToken = localStorage.getItem('token');
    return (authToken !== null) ? true : false;
  }

  getUserId(){
    return JSON.parse(localStorage.getItem('token')!);
  }
}