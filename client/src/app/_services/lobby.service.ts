import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map, retry } from "rxjs/operators";
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

    fetchLobby(id: number){

    }

    fetchAllLobbies(){
        return this.http.get(this.baseUrl + 'lobbies', {
            responseType: 'json'
        }).pipe(
            map(responseData => {
                console.log(responseData);
            })
        );
    }

}