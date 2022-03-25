import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { map } from "rxjs/operators";
import { GamesService } from "../_services/games.service";

@Injectable({providedIn: 'root'})
export class GamesResolver implements Resolve<any>{
    constructor(private gamesService: GamesService){}
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.gamesService.getActiveGames().pipe(
            map(response => {
                return response;
            })
        )
    }
}