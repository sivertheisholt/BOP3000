import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { Game } from "../_models/game.model";

@Injectable({providedIn: 'root'})
export class GamesService{

    baseUrl = 'https://localhost:5001/api/';
    
    constructor(private http: HttpClient){}
    getActiveGames(){
        return this.http.get<Game[]>(this.baseUrl + 'apps/active');
    }

    fetchGame(id: number){
        return this.http.get<Game>(this.baseUrl + 'apps/' + id).pipe(
            map(response => {
                let gameInfo: Game;
                gameInfo = response;
                return gameInfo;
            })
        );
    }

    searchGame(input: string){
        return this.http.get<Game>(this.baseUrl + 'apps/search', {
            params: {
                name: input
            }
        }).pipe(
            map(response => {
                console.log(response);
                let gameInfo: Game;
                gameInfo = response;
                return gameInfo;
            })
        );
    }
}