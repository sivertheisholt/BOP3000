import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { LobbyService } from '../_services/lobby.service';

@Injectable({
  providedIn: 'root'
})
export class LobbyGuard implements CanActivate {
  constructor(private lobbyService: LobbyService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Promise<boolean> {
      return new Promise((resolve) => {
        this.lobbyService.getLobbyStatus().subscribe(
            (res) => {
                if(res.lobbyId == route.params.id && !res.inQueue){
                    resolve(true);
                } else {
                    this.router.navigate(['home']);
                    resolve(false);
                }
            }
        )
      })
    }
}
