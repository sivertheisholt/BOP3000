import { HttpBackend, HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class ConnectionService{
    baseUrl = environment.apiUrl;
    private httpClient: HttpClient;

    constructor(private handler: HttpBackend, private http: HttpClient){
        this.httpClient = new HttpClient(this.handler);
    }

    connectToSteam(){
          return this.httpClient.post<any>(this.baseUrl + 'api/accounts/steam', "", {
              headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded'),
              responseType: 'text' as 'json',
              observe: 'response' as 'body'
          });
    }

    connectToDiscord(){
          return this.httpClient.post<any>(this.baseUrl + 'accounts/discord', '', {
            headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded'),
            responseType: "text" as "json",
            observe: "response" as "body"
          });
    }

    disconnectFromDiscord(){
        return this.http.patch(this.baseUrl + 'members/discord/unlink', '');
    }

    disconnectFromSteam(){
        return this.http.patch(this.baseUrl + 'members/steam/unlink', '');
    }

    onDiscordSuccess(){
        return this.http.patch(this.baseUrl + 'accounts/discord-success', {});
    }

    onSteamSuccess(){
        return this.http.patch(this.baseUrl + 'accounts/steam-success', {});
    }
}