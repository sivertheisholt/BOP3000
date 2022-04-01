import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { LobbyService } from '../_services/lobby.service';

@Injectable({
  providedIn: 'root'
})
export class ArchivedLobbyGuard implements CanActivate {
  constructor(private lobbyService: LobbyService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
    return new Promise((resolve) => {
        this.lobbyService.getLobbyStatus(route.params.id).subscribe(
            (res) => {
                if(res){
                    resolve(true);
                } else {
                    this.router.navigate(['lobby', route.params.id]);
                    resolve(false);
                }
            })
        })
    }
}