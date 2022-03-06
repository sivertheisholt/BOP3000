import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, retry } from "rxjs/operators";
import { Lobby } from "../_models/lobby.model";

@Injectable({providedIn: 'root'})
export class LobbyService{

    baseUrl = 'https://localhost:5001/api/';

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
}