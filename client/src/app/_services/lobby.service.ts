import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, retry } from "rxjs/operators";
import { Lobby } from "../_models/lobby.model";
import { QueueStatus } from "../_models/queuestatus.model";
import { environment } from '../../environments/environment';

@Injectable({providedIn: 'root'})
export class LobbyService{
    baseUrl = environment.apiUrl;

    constructor(private http: HttpClient){}

    postLobby(body : Lobby){
        return this.http.post<Lobby>(this.baseUrl + 'lobbies', body)
        .pipe(
            map(response => {
                return response;
            })
        );
    }

    fetchLobby(id: number){
        return this.http.get<Lobby>(this.baseUrl + 'lobbies/' + id);
    }

    fetchAllLobbies(){
        return this.http.get<Lobby[]>(this.baseUrl + 'lobbies');
    }

    fetchAllLobbiesWithGameId(id: number){
        return this.http.get<Lobby[]>(this.baseUrl + 'lobbies/game/' + id);
    }

    getLobbyStatus(){
        return this.http.get<QueueStatus>(this.baseUrl + 'members/lobby-status').pipe(
            map((response) => {
                return response;
            })
        )
    }

    fetchLobbyWithId(id: number){
        return this.http.get<Lobby>(this.baseUrl + 'lobbies/' + id).pipe(
            map((response) => {
                return response;
            })
        );
    }
}