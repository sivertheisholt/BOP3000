import { HttpClient, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ReplaySubject, Subject } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';

  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {
  }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map((res: User) => {
        const user = res;
        if(user) {
          this.initCurrentUser(res);
        }
      })
    );
  }

  register(model: any) {
    console.log(model);
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((res: User) => {
        const user = res;
        if(user) {
          this.initCurrentUser(res);
        }
      })
    )
  }

  initCurrentUser(user: User) {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  setCurrentUser(user: User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(undefined);
  }

  get isLoggedIn(): boolean{
    let authToken = localStorage.getItem('user');
    return (authToken !== null) ? true : false;
  }
}