import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";

@Injectable({providedIn: 'root'})
export class GamesService{

    baseUrl = 'https://localhost:5001/api/';
    
    constructor(private http: HttpClient){}
    fetchGames(){
        return this.http
            .get('')
            .pipe(
                map(responseData => {

        }));
    }
}