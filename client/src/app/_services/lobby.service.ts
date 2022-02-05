import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { retry } from "rxjs/operators";
import { Lobby } from "../_models/lobby.model";

@Injectable({providedIn: 'root'})
export class LobbyService{

    baseUrl = 'https://localhost:5001/api/';

    constructor(private http: HttpClient){}

    postLobby(body : Lobby){
        console.log(JSON.stringify(body));
        return this.http.post<Lobby>(this.baseUrl + 'lobbies', JSON.stringify(body))
        .pipe(
            retry(1)
        )
    }

}