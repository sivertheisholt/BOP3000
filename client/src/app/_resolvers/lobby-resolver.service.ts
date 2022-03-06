import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { map } from "rxjs/operators";
import { LobbyService } from "../_services/lobby.service";

@Injectable({providedIn: 'root'})
export class LobbyResolver implements Resolve<any> {
    constructor(private lobbyService: LobbyService){}
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.lobbyService.fetchLobby(route.params.id).pipe(
            map(response => {
                return response;
        }));
    }
}